<h1 align="center" style="border-bottom: none;">Selection zone📦 </h1>
<h3 align="center">Selection zone for unity 3D</h3>
<p align="center">
  <a href="https://github.com/semantic-release/semantic-release/actions?query=workflow%3ATest+branch%3Amaster">
    <img alt="Build states" src="https://github.com/semantic-release/semantic-release/workflows/Test/badge.svg">
  </a>
  <a href="https://github.com/semantic-release/semantic-release/actions?query=workflow%3ATest+branch%3Amaster">
    <img alt="semantic-release: angular" src="https://img.shields.io/badge/semantic--release-angular-e10079?logo=semantic-release">
  </a>
  <a href="LICENSE">
    <img alt="License" src="https://img.shields.io/badge/License-MIT-blue.svg">
  </a>
</p>
<p align="center">
  <a href="package.json">
    <img alt="Version" src="https://img.shields.io/github/package-json/v/OpenSourceUnityPackage/SelectionZone">
  </a>
  <a href="#LastActivity">
    <img alt="LastActivity" src="https://img.shields.io/github/last-commit/OpenSourceUnityPackage/SelectionZone">
  </a>
</p>

## What is it ?
Selection zone is a package based on [article](https://hyunkell.com/blog/rts-style-unit-selection-in-unity-5/) of Jeff Zimmer to select multiple entity in unity 3D.

## How to use ?
To use it, create a base class for you entities that inheriting from interface ISelectable and define it's behaviour on entity is selected.
```C#
using EntSelect;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{
  [...]
}
```
Next, you need define a script inheriting from UnitSelection<T> and replace T with your selectable entitie created above and include it in your scene.
```C#
using EntSelect;

public class UnitSelection : UnitSelection<Unit> 
{
}
```

You can now select your entities.

You can change style of selection zone in Utils class
 