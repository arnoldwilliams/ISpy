# I Spy with My Little Eye 🕵️

A children's "I Spy" game built with **.NET MAUI (.NET 10, C#)**.

Each round shows a jumbled scene packed with pictures and a checklist of the
pictures to find. Tap a picture in the scene when you spot it — the matching
card on the checklist gets a tick. Find them all to win the round.

## Features

- **75 hand-drawn item pictures** bundled with the app, extracted from the
  original *I Spy with My Little Eye Checklist* artwork (see
  `Resources/Images`).
- **Randomised rounds** — every round picks 6 items to find plus 6 decoys, so
  the hunt is different each time.
- **Jumbled scene layout** — pictures are scattered, sized to their own aspect
  ratio and rotated a little, so they are fun to pick out.
- **Checklist with ticks** — the cards show the picture and name; a tick
  appears when you find it.
- **Gentle feedback** — tapping a decoy says "not on the list" instead of
  penalising the player.
- **Responsive** — the scene is laid out in normalised coordinates, so it
  adapts to phones, tablets and desktop windows.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MAUI workload: `dotnet workload install maui`
- Platform tooling for whichever target you build (Android SDK, Xcode, or
  Windows App SDK)

## Build and run

```bash
cd ISpy

# Android
dotnet build -t:Run -f net10.0-android

# iOS (macOS only)
dotnet build -t:Run -f net10.0-ios

# macOS (Mac Catalyst)
dotnet build -t:Run -f net10.0-maccatalyst

# Windows
dotnet build -t:Run -f net10.0-windows10.0.19041.0
```

## Project layout

```
ISpy/
├── Models/
│   ├── SpyItem.cs          # one item picture (key, name, pixel size)
│   ├── PlacedItem.cs       # an item placed in the scene (normalised coords)
│   └── ItemCatalog.cs      # the 75 bundled items
├── Services/
│   └── SceneGenerator.cs   # builds a jumbled, non-overlapping round
├── ViewModels/
│   ├── ObservableObject.cs
│   ├── SceneItemViewModel.cs
│   ├── ChecklistItemViewModel.cs
│   └── GameViewModel.cs    # round state, taps, scoring, win condition
├── Views/
│   ├── GamePage.xaml       # scene + checklist + new round
│   └── GamePage.xaml.cs    # lays the scene out onto the canvas
├── Resources/
│   ├── Images/             # 75 item pictures
│   ├── AppIcon/            # app icon
│   └── Splash/             # splash screen
└── Platforms/              # Android, iOS, Mac Catalyst, Windows
```

## How a round works

1. `SceneGenerator.CreateRound` shuffles the catalog, takes 6 targets and 6
   decoys, and lays them out on a 3-column grid with per-item aspect fitting,
   small position jitter and a little rotation.
2. `GamePage` reads the normalised positions and converts them to absolute
   `AbsoluteLayout` bounds whenever the scene canvas changes size.
3. Tapping a picture calls `GameViewModel.TapItem`. If the picture is a target
   it is ticked off; when all targets are found the round is won.

## Credits

Item artwork originates from the *I Spy with My Little Eye Checklist*
worksheet (Twinkl). The images are bundled here for use in this children's
game.
