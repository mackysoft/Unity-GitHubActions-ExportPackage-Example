using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MackySoft.ExportPackageExample.Editor {
	public static class UnityPackageExporter {

		const string k_PackageRoot = "MackySoft";
		const string k_PackageName = "ExportPackageExample";
		const string k_TargetPath = "Dist";
		const string k_SearchPattern = "*";

		public static void Export () {
			string exportPath = GetExportPath();
			string[] assetPaths = GetAssetPaths();

			ExportPackage(exportPath,assetPaths);
			ExportVersion($"{k_TargetPath}/version");
		}

		public static string GetExportPath () {
			string version = Environment.GetEnvironmentVariable("UNITY_PACKAGE_VERSION");
			string fileName = !string.IsNullOrEmpty(version) ? $"{k_PackageName}.{version}.unitypackage" : $"{k_PackageName}.unitypackage";
			return $"{k_TargetPath}/{fileName}";
		}

		public static string[] GetAssetPaths () {
			string path = Path.Combine(Application.dataPath,k_PackageRoot);
			string[] assetPaths = Directory.EnumerateFiles(path,k_SearchPattern,SearchOption.AllDirectories)
				.Where(x => Path.GetExtension(x) == ".cs" || Path.GetExtension(x) == ".meta" || Path.GetExtension(x) == "asmdef")
				.Select(x => "Assets" + x.Replace(Application.dataPath,"").Replace(@"\","/"))
				.ToArray();
			return assetPaths;
		}

		public static string ExportPackage (string exportPath,string[] assetPaths) {
			var dir = new FileInfo(exportPath).Directory;
			if (dir != null && !dir.Exists) {
				dir.Create();
			}

			AssetDatabase.ExportPackage(assetPaths,exportPath,ExportPackageOptions.Default);
			return Path.GetFullPath(exportPath);
		}

		public static string ExportVersion (string exportPath) {
			var dir = new FileInfo(exportPath).Directory;
			if (dir != null && !dir.Exists) {
				dir.Create();
			}

			File.WriteAllText(exportPath,k_PackageName);
			return Path.GetFullPath(exportPath);
		}

	}
}