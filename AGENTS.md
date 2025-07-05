# Agent Instructions

## Build and test

This repository requires the .NET SDK 9.0.100. Use the provided script to install the SDK and add it to the PATH before building or testing:

```bash
./dotnet-install.sh --version 9.0.100 --install-dir "$HOME/dotnet"
export PATH="$HOME/dotnet:$PATH"
```

Install LibreOffice so that the UNO native libraries (`libuno_*.so`) are
available. On Ubuntu run:

```bash
sudo apt install libreoffice
```

Before building, source the SDK environment script so that `OO_SDK_HOME` and
other variables are defined:

```bash
source /usr/lib/libreoffice/sdk/setsdkenv_unix.sh
```

Because the Linux packages no longer include the .NET UNO bindings you must
build LibreOffice yourself. Example steps on Debian/Ubuntu:

```bash
sudo apt install git build-essential clang python3 ninja-build \
    libkrb5-dev libgtk-3-dev libcups2-dev dotnet-sdk-8.0 \
    autoconf automake libtool pkg-config gperf libxslt1-dev
git clone --depth=1 https://git.libreoffice.org/core libreoffice-core
cd libreoffice-core
./autogen.sh --enable-release-build --with-dotnet=$(which dotnet) --without-java
make -j$(nproc)
```

LibreOffice packages on Linux no longer ship the .NET UNO bindings. To obtain
them you must build LibreOffice from source with `--with-dotnet`. After the
build finishes, register the local NuGet feed at `instdir/sdk/dotnet` and
install the `LibreOffice.Bindings` package. The native
libraries (`libuno_*.so`) are installed under `/usr/lib/libreoffice/program`.
Verify that directory contains the libraries:

```bash
ls /usr/lib/libreoffice/program/libuno_*.so
```

After building register the local feed and install the bindings:

```bash
dotnet nuget add source instdir/sdk/dotnet --name LO
dotnet add package LibreOffice.Bindings --prerelease
```

Make the native libraries discoverable by setting `LD_LIBRARY_PATH` before
running tests:

```bash
export LD_LIBRARY_PATH=/usr/lib/libreoffice/program:$LD_LIBRARY_PATH
```

If the CLI assemblies are missing, the UNO C# bindings will not work and tests will fail.

Run tests with:

```bash
dotnet test
```

The file `demo.docx` is used by tests to verify conversion to PDF through the UNO API.

## Notes
- Interactions with LibreOffice must use the UNO API and **not** the `soffice` CLI.
- Ensure all programmatic checks are executed even for documentation changes.
