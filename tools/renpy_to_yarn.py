#!/usr/bin/env python3
import re
import os
import sys
import argparse
import pathlib
from typing import List, Dict, Tuple

LABEL_RE = re.compile(r"^\s*label\s+([a-zA-Z0-9_]+)\s*:\s*$")
JUMP_RE = re.compile(r"^\s*(?:jump|call)\s+([a-zA-Z0-9_]+)\s*$")
ASSIGN_RE = re.compile(r"^\s*(?:\$\s*)?([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*(True|False)\s*$")
MENU_RE = re.compile(r"^\s*menu\s*:\s*$")
MENU_OPT_RE = re.compile(r"^\s*\"(.+?)\"\s*:\s*$")
IF_RE = re.compile(r"^\s*if\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*:\s*$")
DIALOG_RE = re.compile(r"^\s*(?:[a-zA-Z_][a-zA-Z0-9_]*\s+)?\"(.+?)\"\s*$")
COMMENT_RE = re.compile(r"^\s*#")


def sanitize_title(name: str) -> str:
    return re.sub(r"[^a-zA-Z0-9_]+", "_", name)


def parse_blocks(lines: List[str]) -> Dict[str, List[Tuple[int, str]]]:
    blocks: Dict[str, List[Tuple[int, str]]] = {}
    current_label = None
    for idx, line in enumerate(lines, start=1):
        m = LABEL_RE.match(line)
        if m:
            current_label = m.group(1)
            blocks[current_label] = []
            continue
        if current_label is not None:
            blocks[current_label].append((idx, line.rstrip("\n")))
    return blocks


def detect_indent(line: str) -> int:
    return len(line) - len(line.lstrip(" \t"))


def convert_label_to_yarn(label: str, content: List[Tuple[int, str]]) -> Tuple[str, List[Tuple[str, int, bool]]]:
    """Return yarn text for node and a list of flag sets (flag, line, value)."""
    out: List[str] = []
    flag_sets: List[Tuple[str, int, bool]] = []

    out.append(f"title: {label}")
    out.append("---")

    i = 0
    n = len(content)
    while i < n:
        line_no, line = content[i]
        if COMMENT_RE.match(line):
            i += 1
            continue

        am = ASSIGN_RE.match(line)
        if am:
            flag = am.group(1)
            val = am.group(2) == "True"
            out.append(f"<<command SetFlag(\"{flag}\", {'true' if val else 'false'})>>")
            flag_sets.append((flag, line_no, val))
            i += 1
            continue

        jm = JUMP_RE.match(line)
        if jm:
            target = jm.group(1)
            out.append(f"[[Go to {target}|{target}]]")
            i += 1
            continue

        im = IF_RE.match(line)
        if im:
            flag = im.group(1)
            base_indent = detect_indent(line)
            out.append(f"<<if HasFlag(\"{flag}\")>>")
            i += 1
            # emit nested lines until dedent
            while i < n:
                lno, l = content[i]
                ind = detect_indent(l)
                if l.strip() == "":
                    i += 1
                    continue
                if ind <= base_indent:
                    break
                # Try to preserve dialog lines within if
                dm = DIALOG_RE.match(l)
                if dm:
                    out.append(dm.group(1))
                else:
                    # include as comment-style
                    out.append(f"// {l.strip()}")
                i += 1
            out.append("<<endif>>")
            continue

        mm = MENU_RE.match(line)
        if mm:
            base_indent = detect_indent(line)
            i += 1
            # parse options
            while i < n:
                lno, l = content[i]
                if l.strip() == "":
                    i += 1
                    continue
                ind = detect_indent(l)
                if ind <= base_indent:
                    break
                om = MENU_OPT_RE.match(l)
                if om:
                    text = om.group(1)
                    opt_indent = detect_indent(l)
                    i += 1
                    # parse block after option to find jump/call
                    target = None
                    while i < n:
                        lno2, l2 = content[i]
                        if l2.strip() == "":
                            i += 1
                            continue
                        ind2 = detect_indent(l2)
                        if ind2 <= opt_indent:
                            break
                        jm2 = JUMP_RE.match(l2)
                        if jm2 and target is None:
                            target = jm2.group(1)
                        # capture flag assignment inside option
                        am2 = ASSIGN_RE.match(l2)
                        if am2:
                            flag = am2.group(1)
                            val = am2.group(2) == "True"
                            out.append(f"<<command SetFlag(\"{flag}\", {'true' if val else 'false'})>>")
                            flag_sets.append((flag, lno2, val))
                        i += 1
                    if target:
                        out.append(f"[[{text}|{target}]]")
                    else:
                        out.append(f"[[{text}]]")
                    continue
                else:
                    # unknown line inside menu, skip gracefully
                    i += 1
            continue

        dm = DIALOG_RE.match(line)
        if dm:
            out.append(dm.group(1))
            i += 1
            continue

        # default. keep as comment to not lose info
        if line.strip():
            out.append(f"// {line.strip()}")
        i += 1

    out.append("===")
    return "\n".join(out) + "\n", flag_sets


def main():
    parser = argparse.ArgumentParser(description="Convert a Ren'Py script to Yarn skeleton nodes")
    parser.add_argument("--input", required=True, help="Path to renpy script.txt")
    parser.add_argument("--outdir", required=True, help="Output directory for yarn files")
    parser.add_argument("--csv", required=False, default=None, help="Optional path to write flags.csv")
    args = parser.parse_args()

    with open(args.input, "r", encoding="utf-8") as f:
        lines = f.readlines()

    blocks = parse_blocks(lines)
    outdir = pathlib.Path(args.outdir)
    outdir.mkdir(parents=True, exist_ok=True)

    all_flag_rows: List[Tuple[str, str, int, bool]] = []

    for label, content in blocks.items():
        yarn_text, flag_sets = convert_label_to_yarn(label, content)
        fname = sanitize_title(label) + ".yarn"
        with open(outdir / fname, "w", encoding="utf-8") as wf:
            wf.write(yarn_text)
        for flag, line_no, val in flag_sets:
            all_flag_rows.append((label, flag, line_no, val))

    if args.csv:
        import csv
        with open(args.csv, "w", newline="", encoding="utf-8") as cf:
            w = csv.writer(cf)
            w.writerow(["label", "flag", "line", "value"])
            for row in all_flag_rows:
                w.writerow(row)

    print(f"Converted {len(blocks)} labels into Yarn nodes at {outdir}")
    if args.csv:
        print(f"Flag sets CSV written to {args.csv}")

if __name__ == "__main__":
    main()
