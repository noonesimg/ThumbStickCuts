# ThumbStickCuts
A simple WPF app that allows to bind shortcuts to a joystick thumbsticks.

## But why?
I'm daily driving a Steamdeck as a portable windows PC. 

While you can bind different keys and shortcuts using something like [HandheldCompanion](https://github.com/Valkirie/HandheldCompanion).
It lacks the ability to quickly change bindings.

This little app on the other hand allows one to bind 72 different shortcuts in a single config file,
and quickly switch between configs.

## How to use it with my windows handheld?
I'm using it in combination with [HandheldCompanion](https://github.com/Valkirie/HandheldCompanion)
with any bindings removed from thumbsticks

## How to choose a shortcut? 
Use `Right stick` to select a zone.

Use `Left stick` to select an action.

`Right stick click` switches to the next layout

`Left stick click` toggles the window visibility


## How do I setup my custom bindings?
Look at the [default config examples](https://github.com/noonesimg/ThumbStickCuts/tree/main/DefaultConfigs).
then place your `config.json` file into the `configs` folder.

There're 9 zones that you can choose from:
```json
[
  "Center",
  "Left",
  "Right",
  "Top",
  "Bottom",
  "TopLeft",
  "TopRight"
  "BottomLeft",
  "BottomRight",
]
```
Each zone can hold up to 8 shortcuts.

## How do I specify a shortcut?
each shortcut consists of two fields: `Action` and `Icon`
```json
{ "Action": "Ctrl+c", "Icon": "c" }
```
You can simply type: `"Ctrl+Shift+s"` into the `Action` field

To specify multiple shortcuts at once split them with a comma:
```json
{ "Action": "Ctrl+k,Ctrl+o", "Icon": "c" }
```

Not that it's super usefull, but you can even bind whole sentences like that:
```json
{ "Action": "Shift+h,e,l,l,o, ,w,o,r,l,d", "Icon": "c" }
```

Alternatively you can use `VirtualKeyCode` values from [InputSimulatorCore](https://github.com/cwevers/InputSimulatorCore/blob/master/WindowsInput/Native/VirtualKeyCode.cs)
```json
{ "Icon": "*", "Action": "Ctrl+OEM_COMMA" } 
```

## How do I specify a shortcut icon?
This app uses `RobotoMono Nerd Font Mono` from [nerd-fonts](https://github.com/ryanoasis/nerd-fonts)

Please make sure your text editor of choise has this font installed.

You can open the font file with something like [Character Map UWP](https://apps.microsoft.com/detail/character-map-uwp/9WZDNCRDXF41?hl=en-US&gl=US)

Simply copy paste the glyph you like into the "Icon" field.


## I don't have a handheld, can I still use this?

Sure, you can use it with any windows compatible gamepad that has thumbsticks, although I don't know why would you do that :)




