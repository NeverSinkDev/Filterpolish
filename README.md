# Filterpolish

![Build passing?](https://api.travis-ci.org/NeverSinkDev/Filterpolish.svg?branch=master)

[work in progress] This is a small tool that works with .filter files. It reads them into memory and rebuilds them, while optimizing the structure. It also provides a small set of tools for creating automatic subversions. The syntax can be adapted for other similar markup languages or rule-based systems. Currently it only works with PoE. filter files.

*This tool is not designed to replace Notepad++ or Filtration for .filter editing. It's designed to help filter-makers automate processes and repetitive tasks.*

What it currently does:

- Opens .filter files, parses them, creates an internal representation of the filter (split by "Line" and "Entry").

- Removes redundancies and cleans a bit - aligns lines, removes filler characters, adjusts indents

- Sorts the lines within each Entry.

- Auto-creates filter subversions based upon "version tags" ( as seen in https://github.com/NeverSinkDev/NeverSink-Filter )

- Generates style subversions based on style files (still work in progress)

- Creates a table of contents and adjusts the numbering of header-comments in the filter

- Creates and handles Stylesheets, that allows quick generation of filters with different visual styles

- Provides a GUI to edit and view the StyleSheets.

- Informs if the parsing failed. Useful for testing versions for upcoming patches. 

- Preserves comments and structure.

- Provides a link to poe.ninja and crawls economy data

- Allows the quick and comfortable resorting of tier-lists

- Creates tier-list based changelogs

- More
