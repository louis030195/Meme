using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;

namespace Editor
{
    public static class Builds
    {
        private static readonly string[] Scenes = {
	        "Assets/Scenes/Main.unity",
	        "Assets/Scenes/Fractal.unity",
            "Assets/Scenes/RandomWalk.unity",
        };
        
        private static void ParseCommandLineArguments(out Dictionary<string, string> providedArguments)
        {
            providedArguments = new Dictionary<string, string>();
            var args = Environment.GetCommandLineArgs();

            Console.WriteLine(
                "\n" +
                "###########################\n" +
                "#    Parsing settings     #\n" +
                "###########################\n" +
                "\n"
            );

            // Extract flags with optional values
            for (int current = 0, next = 1; current < args.Length; current++, next++)
            {
                // Parse flag
                var isFlag = args[current].StartsWith("-");
                if (!isFlag) continue;
                var flag = args[current].TrimStart('-');

                // Parse optional value
                var flagHasValue = next < args.Length && !args[next].StartsWith("-");
                var value = flagHasValue ? args[next].TrimStart('-') : "";
                // bool secret = Secrets.Contains(flag);
                var secret = false;
                var displayValue = secret ? "*HIDDEN*" : "\"" + value + "\"";

                // Assign
                Console.WriteLine($"Found flag \"{flag}\" with value {displayValue}.");
                providedArguments.Add(flag, value);
            }
        }
        
        private static Dictionary<string, string> GetValidatedOptions()
        {
            ParseCommandLineArguments(out var validatedOptions);

            if (!validatedOptions.ContainsKey("projectPath"))
            {
                Console.WriteLine("Missing argument -projectPath");
                EditorApplication.Exit(110);
            }

            if (!validatedOptions.TryGetValue("buildTarget", out var buildTarget))
            {
                Console.WriteLine("Missing argument -buildTarget");
                EditorApplication.Exit(120);
            }

            if (!Enum.IsDefined(typeof(BuildTarget), buildTarget ?? string.Empty))
            {
                EditorApplication.Exit(121);
            }

            // if (!validatedOptions.ContainsKey("customBuildPath"))
            // {
            //     Console.WriteLine("Missing argument -customBuildPath");
            //     EditorApplication.Exit(130);
            // }
            
            return validatedOptions;
        }

        // Public main entry point of all builds, called directly
        [UsedImplicitly]
        public static void BuildOptions()
        {
            // Gather values from args
            var options = GetValidatedOptions();

            // Set version for this build
            PlayerSettings.bundleVersion = options["buildVersion"];
            // PlayerSettings.macOS.buildNumber = options["buildVersion"];
            PlayerSettings.Android.bundleVersionCode = int.Parse(options["androidVersionCode"]);

            // Apply build target
            var buildTarget = (BuildTarget) Enum.Parse(typeof(BuildTarget), options["buildTarget"]);
            if (buildTarget == BuildTarget.Android)
            {
                EditorUserBuildSettings.buildAppBundle = options["customBuildPath"].EndsWith(".aab");
                if (options.TryGetValue("androidKeystoreName", out var keystoreName) &&
                    !string.IsNullOrEmpty(keystoreName))
                    PlayerSettings.Android.keystoreName = keystoreName;
                if (options.TryGetValue("androidKeystorePass", out var keystorePass) &&
                    !string.IsNullOrEmpty(keystorePass))
                    PlayerSettings.Android.keystorePass = keystorePass;
                if (options.TryGetValue("androidKeyaliasName", out var keyaliasName) &&
                    !string.IsNullOrEmpty(keyaliasName))
                    PlayerSettings.Android.keyaliasName = keyaliasName;
                if (options.TryGetValue("androidKeyaliasPass", out var keyaliasPass) &&
                    !string.IsNullOrEmpty(keyaliasPass))
                    PlayerSettings.Android.keyaliasPass = keyaliasPass;
                // IL2CPP 
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                // Google Play want all CPU especially arm64
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
                PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            }
            else if (buildTarget != BuildTarget.iOS)
            {
                // PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);
            }

            // Custom build
            Build(buildTarget, options["customBuildPath"]);
        }

        // Internal main entry point of all builds
        private static void Build(BuildTarget buildTarget, string filePath) 
        {
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = Scenes,
                locationPathName = filePath,
                target = buildTarget
            };
            var buildSummary = BuildPipeline.BuildPlayer(buildPlayerOptions).summary;
            Console.WriteLine($"Build summary: {buildSummary}");
        }
    }
}
