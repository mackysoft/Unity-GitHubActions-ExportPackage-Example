# Unity & GitHubActions ExportPackage Example

A example project of exporting a package using Unity and GitHub Actions.

> Qiita: [UnityとGitHubActionsを使って自動でunitypackageをエクスポートする](https://qiita.com/makihiro_dev/items/4e2d83ad0d268b43f4a4)


## 🔰 Tutorial

### 1. Write C# to export the package.

Create `UnityPackageExporter.cs` under the `Editor` folder in the project repository, and write the following.
(The package name and other constants should match your project)

```cs:UnityPackageExporter.cs
using System.IO;
using UnityEditor;

namespace ExportPackageExample.Editor {
	public static class UnityPackageExporter {

		// The name of the unitypackage to output.
		const string k_PackageName = "ExportPackageExample";

		// The path to the package under the `Assets/` folder.
		const string k_PackagePath = "ExportPackageExample";

		// Path to export to.
		const string k_ExportPath = "Build";

		public static void Export () {
			ExportPackage($"{k_ExportPath}/{k_PackageName}.unitypackage");
		}

		public static string ExportPackage (string exportPath) {
			// Ensure export path.
			var dir = new FileInfo(exportPath).Directory;
			if (dir != null && !dir.Exists) {
				dir.Create();
			}

			// Export
			AssetDatabase.ExportPackage(
				$"Assets/{k_PackagePath}",
				exportPath,
				ExportPackageOptions.Recurse
			);

			return Path.GetFullPath(exportPath);
		}

	}
}
```

This is a fairly concise code, so if you want to customize the package in more detail, please refer to the official documentation.
https://docs.unity3d.com/ScriptReference/AssetDatabase.ExportPackage.html


### 2. Acquire ULF

Acquire the ULF file to activate your Unity license.
Use the following tool to acquire the ULF file.

https://github.com/mackysoft/Unity-ManualActivation


### 3. Register ULF to Secrets

1. Select the `Settings > Secrets` menu in the project repository.
2. Click the `New repository secret` button.
3. Enter "UNITY_LICENSE" in Name and copy and paste the contents of the ULF file in Value.
4. Click the `Add secret` button.

You can now treat the contents of the ULF file as an environment variable while keeping its contents private.

### 4. Write YAML to export unitypackage on GitHub Actions.

Create a `package.yaml` under the repository `.github/workflows/` and write the following.

```yaml:package.yaml
name: Export Package

on:
  pull_request: {}
  push: { branches: [main] }

env:
  UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}

jobs:
  build:
    name: Build UnityPackage
    runs-on: ubuntu-latest
    steps:
      # Checkout
      - name: Checkout repository
        uses: actions/checkout@v2
        with:
          lfs: true

      # Cache
      - name: Cache
        uses: actions/cache@v2
        with:
          path: Library
          key: Library
          restore-keys: Library-

      # Build
      - name: Build .unitypackage
        uses: game-ci/unity-builder@v2
        with:
          unityVersion: 2020.3.1f1
          buildMethod: ExportPackageExample.Editor.UnityPackageExporter.Export # Path to the export method containing the namespace.

      # Upload
      - name: Upload .unitypackage
        uses: actions/upload-artifact@v2
        with:
          name: Unity Package
          path: Build
```


### 5. Exporting

The workflow for exporting unitypackage will run by making a Push and Pull Request to the repository.

If you see a tick mark as shown below, you have succeeded. The exported unitypackage can be acquired from Artifacts in the upper right corner.

![ExportResult](https://user-images.githubusercontent.com/13536348/114300084-80995a00-9af9-11eb-9113-afd9ecf6f7d9.jpg)


#  📔 Author Info

Hiroya Aramaki is a indie game developer in Japan.

- Blog: [https://mackysoft.net/blog](https://mackysoft.net/blog)
- Twitter: [https://twitter.com/makihiro_dev](https://twitter.com/makihiro_dev)


#  📜 License

This repository is under the MIT License.