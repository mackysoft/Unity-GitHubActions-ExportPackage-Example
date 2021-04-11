using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MackySoft.ExportPackageExample.Editor {
	public static class UnityPackageExporter {

		const string k_PackageRoot = "MackySoft";
		const string k_PackageName = "ExportPackageExample";
		const string k_TargetPath = "Build";
		const string k_SearchPattern = "*";

		public static void Export () {
			string exportPath = GetExportPath();
			string[] assetPaths = GetAssetPaths();

			ExportPackage(exportPath,$"{Path.Combine(Application.dataPath,k_PackageRoot)}/");
			ExportVersion($"{k_TargetPath}/version.txt");
		}

		public static string GetExportPath () {
			string version = Environment.GetEnvironmentVariable("UNITY_PACKAGE_VERSION");
			string fileName = !string.IsNullOrEmpty(version) ? $"{k_PackageName}.{version}" : k_PackageName;
			return $"{k_TargetPath}/{fileName}.unitypackage";
		}

		public static string[] GetAssetPaths () {
			string path = Path.Combine(Application.dataPath,k_PackageRoot);
			return new string[] { path };
		}

		public static string ExportPackage (string exportPath,string assetPath) {
			var dir = new FileInfo(exportPath).Directory;
			if (dir != null && !dir.Exists) {
				dir.Create();
			}

			AssetDatabase.ExportPackage(assetPath,exportPath,ExportPackageOptions.Recurse);
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