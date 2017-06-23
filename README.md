# Babylon Text for Unity!

A small utility to manage language files in Unity. Self contained and easy to integrate to your current project.

The idea is simple: Reads the system language and attemps to open that dictionary. If doesn't exist, opens the default one. If that one doesn't exist either, well, you can go to hell. Create some language files already!

Once the dictionary is loaded, you can access it with a public method in the BabylonCore class, or try the lazy version, and use the Unity UI Text "add-on" BabylonText class instead. More details bellow.

## Steps

Copy the contents of Assets/babylon-text to your project's Assets folder (wherever you want). Unity Package coming soon in the releases section.

Create some languaje.json files somewhere in a Resources folder. The filenames must be named after Unity's [System Language Enum](https://docs.unity3d.com/ScriptReference/SystemLanguage.html).

Configure the Path To Bookshelf in the Babylon Core asset (is in resources/config) to point at the folder containing your .json language files. Also you could configure another default fallback SystemLanguage (or leave it in English).

You are ready to go! Use BabylonCore.instance.GetString to, well, get your strings from the selected dictionary. Also, the Babylon Text component will automatically change a Unity UI Text content from keys to values in the selected dictionary.

## Dependencies and conflicts

This project uses json.net as a plugin. If you are using it already in your project, you may want to delete the one that comes with this implementation to avoid conflicts.

## License

MIT license, so feel free to use it in whatever project you want without asking. 

## Collaboration

Feel free to send pull requests if you want to improve/fix this mess.
