# MouseGile 🖱️

MouseGile is a lightweight utility tool that keeps your system active by automatically jiggling the mouse and gently scrolling at regular intervals for a custom duration.

---

## 📥 Download

| Package | Download Link | Description |
| :--- | :--- | :--- |
| **Windows Installer (Velopack)** | [⬇️ **Download MouseGile-win-Setup.exe**](https://github.com/hxni444/MouseGile/releases/latest/download/MouseGile-win-Setup.exe) | Recommended: automatic updates, Start Menu & Desktop shortcuts |
| **Portable ZIP** | [📦 **Download MouseGile-win-Portable.zip**](https://github.com/hxni444/MouseGile/releases/latest/download/MouseGile-win-Portable.zip) | Standalone executable, no installation required |
| **All Releases** | [🏷️ **View GitHub Releases**](https://github.com/hxni444/MouseGile/releases/latest) | Changelog, release notes, and assets |

---

## ✨ Features

- ⏱️ **Real-Time Countdown**: Per-second countdown timer dynamically showing the exact time left.
- 🔄 **Smart Jiggler**: Subtly moves the cursor and triggers minor wheel scrolls every 30 seconds without interfering with your workflow.
- 🏷️ **Interactive Version Pill**: Shows the active version (e.g. `v1.0.0`) and lets you check for and apply updates with a single click.
- 📦 **Velopack Integration**: Seamless installer generation, delta updates, and automatic in-app updates via GitHub Releases.
- 💡 **Status Indicator**: Visual glowing round indicator showing active (green) and idle (red) states.
- 🌙 **Pitch Black Theme**: Modern OLED pure black UI with native dark title bar support.

---

## 🚀 Installation & Updates

### Velopack Installer (Recommended)
1. Download and run [MouseGile-win-Setup.exe](https://github.com/hxni444/MouseGile/releases/latest/download/MouseGile-win-Setup.exe).
2. MouseGile will install and launch automatically.
3. When future versions are published, MouseGile will automatically check for updates on startup or on clicking the **Version Pill** in the top right.

### Portable Version
1. Download [MouseGile-win-Portable.zip](https://github.com/hxni444/MouseGile/releases/latest/download/MouseGile-win-Portable.zip).
2. Extract the archive anywhere and run `MouseGile.exe`.

---

## 🛠️ Building & Packaging with Velopack

### 1. Install the Velopack CLI Tool
```powershell
dotnet tool install -g vpk
```

### 2. Publish the Self-Contained Binary
```powershell
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

### 3. Package the Installer with Velopack
```powershell
vpk pack -u MouseGile -v 1.0.0 -p bin\Release\net8.0-windows\win-x64\publish -e MouseGile.exe --icon mousegile.ico -o Releases
```

### 4. Upload Release to GitHub
```powershell
vpk upload github --repoUrl https://github.com/hxni444/MouseGile --token YOUR_GITHUB_TOKEN
```
*(Or create a Release on GitHub and upload the files from the `Releases/` folder: `MouseGile-win-Setup.exe`, `MouseGile-win-Portable.zip`, `MouseGile-1.0.0-full.nupkg`, `releases.win.json`)*

---

## 🤝 Contributing
Contributions are welcome!
1. Fork the repository
2. Create a new feature branch (`git checkout -b feature-name`)
3. Commit your changes (`git commit -m "Add feature"`)
4. Push to your branch (`git push origin feature-name`)
5. Open a Pull Request

---

## 👨‍💻 Author
Developed by **Hani Al Ziya**
