# UnoSharp

UnoSharp is the simple wrapper to operate LibreOffice by Universal Network Objects (hereinafter UNO).

Classes and Methods are somewhat similar to the Microsoft Office Excel model.

## Example

```cs
// new workbook
var book1 = new Workbook();
book1.Worksheets.Add("new sheet");
book1.SaveAs(@"D:\test1.ods");
book1.Close();

// open workbook
using (var book2 = new Workbook(@"D:\test2.ods"))
{
    Worksheet sheet = book2.Worksheets["Sheet1"];
    string text = sheet.CellAt("A1").Text;
    double value = sheet.CellAt("A1").Value;
}
```

## Setup LibreOffice

To build on Linux you need the .NET 9 SDK and LibreOffice with its development package.

Install the SDK with the provided script:

```bash
./dotnet-install.sh --version 9.0.100 --install-dir "$HOME/dotnet"
export PATH="$HOME/dotnet:$PATH"
```

Install LibreOffice:

```bash
sudo apt install libreoffice
```

Before building, source the SDK environment to configure UNO paths:

```bash
source /usr/lib/libreoffice/sdk/setsdkenv_unix.sh
```

LibreOffice packages for Linux no longer ship the .NET bindings for UNO. To
obtain the required .NET assemblies you must build LibreOffice from source with
the `--with-dotnet` option. After compilation you will find a NuGet package
`LibreOffice.Bindings.<ver>.nupkg` inside `instdir/sdk/dotnet`. Add this local
feed and install the package in this project, then copy the managed
`net_*.dll` files from `instdir/program/dotnet` next to your executable.

```bash
dotnet nuget add source instdir/sdk/dotnet --name LO
dotnet add package LibreOffice.Bindings --prerelease
cp instdir/program/dotnet/net_*.dll ./
```

To build LibreOffice with .NET bindings on Debian/Ubuntu:

To build LibreOffice with .NET bindings on Debian/Ubuntu:

```bash
sudo apt install git build-essential clang python3 ninja-build \
    libkrb5-dev libgtk-3-dev libcups2-dev dotnet-sdk-8.0 \
    autoconf automake libtool pkg-config gperf libxslt1-dev
git clone --depth=1 https://git.libreoffice.org/core libreoffice-core
cd libreoffice-core
./autogen.sh --enable-release-build --with-dotnet=$(which dotnet) --without-java
make -j$(nproc)
```

The UNO native libraries reside in `/usr/lib/libreoffice/program`. Verify that
directory exists:

```bash
ls /usr/lib/libreoffice/program/libuno_*.so
```


Native UNO libraries loaded via `LD_LIBRARY_PATH` include:

```
libuno_sal.so
libuno_salhelpergcc3.so
libuno_cppuhelpergcc3.so
libuno_cppu.so
libuno_ure.so
```

Before building or running tests ensure the native libraries can be found by
setting `LD_LIBRARY_PATH`:

```bash
export LD_LIBRARY_PATH=/usr/lib/libreoffice/program:$LD_LIBRARY_PATH
```

Please match 64bit or 32bit with the application to be executed: If you use 64bit application, choose 64bit LibreOffice.

~~If you execute AnyCPU application on 64bit system, and are not sure whether it will work on 64bit, I recommend to install both 32bit and 64bit.~~
Unfortunately, We cannot install both 32bit and 64bit (otherhand is removed), So if you execute Any CPU application on 64bit system, UnoSharp is throw exception in some case.
