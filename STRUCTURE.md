# Project Structure

root /
├── .git/
├── .gitignore
├── .gitmodules                        # points to the lib submodule
├── NetChat.sln                        # solution: list of all projects
├── Structure.md                       # this file
├── TODO.md
├── Docs/
│   ├── documentation
│   └── plans
├── lib/                               # git submodules, never edited here
│   └── MyLib/
├── src/
│   ├── NetChat.Shared/                # classlib, references nothing of ours
│   │   ├── NetChat.Shared.csproj
│   │   ├── Netcode.cs                 # framing + serialization
│   │   ├── Data.cs                    # shared data types
│   │   │   ├── User
│   │   │   └── Message
│   ├── NetChat.Server/                # console exe, references Shared
│   │   ├── NetChat.Server.csproj
│   │   ├── Program.cs                 # entry file, kept tiny
│   │   └── Server.cs
│   ├── NetChat.Client.Core/           # classlib, references Shared, no UI
│   │   ├── NetChat.Client.Core.csproj
│   │   └── Client.cs
│   └── NetChat.Client.Tui/            # console exe, references Client.Core
│       ├── NetChat.Client.Tui.csproj
│       └── Program.cs                 # entry file for the TUI client
└── tests/
    ├── NetChat.Shared.Tests/
    ├── NetChat.Client.Core.Tests/
    └── NetChat.Server.Tests/

## Dependencies

Shared <── Server
Shared <── Client.Core <── Client.Tui
lib/MyLib <── (whichever project uses it, ideally Shared)

Server and Client.Core never reference each other.

-# LLM helped me type this. HOWEVER I wrote the structure from research.
