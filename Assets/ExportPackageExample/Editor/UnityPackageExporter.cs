using System.IO;
using UnityEditor;

namespace ExportPackageExample.Editor {
	public static class UnityPackageExporter {

		const string k_PackagePath = "ExportPackageExample";
		const string k_PackageName = "ExportPackageExample";
		const string k_TargetPath = "Build";

		public static void Export () {
			ExportPackage($"{k_TargetPath}/{k_PackageName}.unitypackage");
			ExportVersion($"{k_TargetPath}/version.txt");
		}

		public static string ExportPackage (string exportPath) {
			var dir = new FileInfo(exportPath).Directory;
			if (dir != null && !dir.Exists) {
				dir.Create();
			}

			AssetDatabase.ExportPackage(
				$"Assets/{k_PackagePath}",
				exportPath,
				ExportPackageOptions.Recurse
			);

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