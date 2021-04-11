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