# Filterpolish
[work in progress] This is a small tool that works with .filter files. It reads them into memory and rebuilds them, while optimizing the structure. It also provides a small set of tools for creating automatic subversions. The syntax can be adapted for other similar markup languages or rule-based systems. Currently it only works with PoE. filter files.

What it currently does:

- Opens .filter files, parses them, creates an internal representation of the filter (split by "Line" and "Entry"). It's designed in a way that preserves all comments.

- Removes redundancies and cleans a bit - aligns lines, removes filler characters, adjusts indents

- Sorts the lines within each Entry.

- Auto-creates filter subversions based upon "version tags" ( as seen in https://github.com/NeverSinkDev/NeverSink-Filter )

- Creates a table of contents and adjusts the numbering of header-comments in the filter

- Creates and handles Stylesheets, that allows quick generation of filters with different visual styles

- Provides a GUI to edit and view the StyleSheets.

- More
