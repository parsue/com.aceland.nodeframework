# Changelog

All notable changes to this project will be documented in this file.

## [1.0.17] - 2025-02-03

### Modify
- [MonoNode] remove test debug log from ContainerNode

## [1.0.16] - 2025-01-25

### Fixed
- [MonoNode] initial whole tree by root node, and call StartAfterNodeBuilt() after initialization of whole tree.
### Added
- [MonoNode] add extension .MonoNeighbour<T>() and .MonoNeighbour<T>(id).
### Modify
- [Editor] AceLand Project Setting as Tree structure

## [1.0.15] - 2025-01-14

### Fixed
- [MonoNode] ChildNodes not complete ready when ParentNode StartAfterNodeBuilt() invoked. This is caused by non-sequently initialized of Components.  Fixed on LateStart Task control.

## [1.0.14] - 2025-01-02

### Fixed
- [MonoNode] comparing on Node not build not correct. Empty ID of either node will not be equal.

## [1.0.13] - 2025-01-01

### Fixed
- [MonoNode] Registry wrong type through Extension

## [1.0.12] - 2025-01-01

### Modify
- [MonoNode] StartAfterNodeBuilt after Awake
- [MonoNode] Auto build Node Structure

### Fixed
- [MonoNode] Null checking on Extension Functions

## [1.0.11] - 2024-11-26

### Modify
- [Editor] Undo functional for Project Settings

## [1.0.9] - 2024-11-26

### Add
- add Getter for MonoNode.
[package.json](package.json)
## [1.0.7] - 2024-11-25

### Fixed
- MonoNode register comparer issue, always node exists or no error on exists.

### Add
- add Context Menu to Enable/Disable Auto Register of all children of selected Game Object. 

## [1.0.4] - 2024-11-24

First public release. If you have an older version, please update or re-install.   
For detail please visit and bookmark our [GitBook](https://aceland-workshop.gitbook.io/aceland-unity-packages/)
