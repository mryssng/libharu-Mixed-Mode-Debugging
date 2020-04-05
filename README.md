# **Mixed Mode Debugging of libharu with Visual Studio 2019**

## Precondition
Download the source code from these linkes.
 - libharu ver2.3.0: [http://libharu.org/](http://libharu.org/)
 - zlib ver1.2.11: [https://zlib.net/](https://zlib.net/)
 - libping ver1.6.37: [http://www.libpng.org/pub/png/libpng.html](http://www.libpng.org/pub/png/libpng.html)

It required that libpng and zlib has been built with VS2019.<br>
Reference: [Building libpng with Visual Studio 2019](https://github.com/mryssng/libpng-VisualStudio2019)

"libpng16.lib" and "zlib.lib" needs to be copied to "libharu_Mixed_Mode_Debugging\libharu_dll\libharu".
libpng16.lib is copied from "libpng-VisualStudio2019\lpng1637\projects\vstudio\x64\Release Library\"

	    libharu_Mixed_Mode_Debugging/         # root for building
	    └─ libharu_dll/                       # libharu
	        └─ libharu/                       # libharu
	            ├─ libpng16.lib               # copied from libpng-VisualStudio2019\lpng1637\projects\vstudio\x64\Release Library\libpng16.lib
	            └─ zlib.lib                   # copied from libpng-VisualStudio2019\lpng1637\projects\vstudio\x64\Release Library\zlib.lib
    
## Edit hpdf.cs in the project "libharu_app"
1. Change all "[DllImport("libhpdf.dll")]" part to "[DllImport("libhpdf.dll", CallingConvention = CallingConvention.Cdecl)]" 

2. Change the definition of the following methods as below.<br>

```
    * Line #340
    private extern static string HPDF_GetVersion();
    to
    private extern static IntPtr HPDF_GetVersion();

    * Line #419
    private extern static string HPDF_LoadType1FontFromFile(IntPtr pdf, string afmfilename, string pfmfilename);
    to
    private extern static IntPtr HPDF_LoadType1FontFromFile(IntPtr pdf, string afmfilename, string pfmfilename);

    * Line #423
    private extern static string HPDF_LoadTTFontFromFile(IntPtr pdf, string file_name, int embedding);
    to
    private extern static IntPtr HPDF_LoadTTFontFromFile(IntPtr pdf, string file_name, int embedding);

    * Line #427
    private extern static string HPDF_LoadTTFontFromFile2(IntPtr pdf, string file_name, uint index, int embedding);
    to
    private extern static IntPtr HPDF_LoadTTFontFromFile2(IntPtr pdf, string file_name, uint index, int embedding);

    * Line #505
    private extern static string HPDF_GetInfoAttr(IntPtr pdf, HPdfInfoType type);
    to
    private extern static IntPtr HPDF_GetInfoAttr(IntPtr pdf, HPdfInfoType type);

    * Line #1711
    private extern static string HPDF_Font_GetFontName(IntPtr hfont);
    to
    private extern static IntPtr HPDF_Font_GetFontName(IntPtr hfont);

    * Line #1714
    private extern static string HPDF_Font_GetEncodingName(IntPtr hfont);
    to
    private extern static IntPtr HPDF_Font_GetEncodingName(IntPtr hfont);
      
    * Line #2060
    private extern static string HPDF_Image_GetColorSpace(IntPtr image);
    to
    private extern static IntPtr HPDF_Image_GetColorSpace(IntPtr image);
```

3. Change the implementation of the following methods as below.<br>
```
    public static string HPdfGetVersion() {
        return HPDF_GetVersion();
    }
    to
    public static string HPdfGetVersion() {
        return Marshal.PtrToStringAnsi(HPDF_GetVersion());
    }
```
```
    public string LoadType1FontFromFile(string afmfilename,
            string pfmfilename) {
        string font_name;

        font_name = HPDF_LoadType1FontFromFile(hpdf, afmfilename, pfmfilename);
        return font_name;
    }
    to
    public string LoadType1FontFromFile(string afmfilename,
            string pfmfilename) {
        IntPtr  font_name;

        font_name = HPDF_LoadType1FontFromFile(hpdf, afmfilename, pfmfilename);
        return Marshal.PtrToStringAnsi(font_name);
    }
```
```
    public string LoadTTFontFromFile(string file_name, bool embedding) {
        string font_name;
        int emb;

        if (embedding)
            emb = HPDF_TRUE;
        else
            emb = HPDF_FALSE;

        font_name = HPDF_LoadTTFontFromFile(hpdf, file_name, emb);
        return font_name;
    }
    to
    public string LoadTTFontFromFile(string file_name, bool embedding) {
        IntPtr font_name;
        int emb;

        if (embedding)
            emb = HPDF_TRUE;
        else
            emb = HPDF_FALSE;

        font_name = HPDF_LoadTTFontFromFile(hpdf, file_name, emb);
        return Marshal.PtrToStringAnsi(font_name);
    }
```
```
    public string LoadTTFontFromFile2(string file_name, uint index,
            bool embedding) {
        string font_name;
        int emb;

        if (embedding)
            emb = HPDF_TRUE;
        else
            emb = HPDF_FALSE;

        font_name = HPDF_LoadTTFontFromFile2(hpdf, file_name, index, emb);
        return font_name;
    }
    to
    public string LoadTTFontFromFile2(string file_name, uint index,
            bool embedding) {
        IntPtr font_name;
        int emb;

        if (embedding)
            emb = HPDF_TRUE;
        else
            emb = HPDF_FALSE;

        font_name = HPDF_LoadTTFontFromFile2(hpdf, file_name, index, emb);
        return Marshal.PtrToStringAnsi(font_name);
    }
```
```
    public string GetInfoAttr(HPdfInfoType type) {
        return HPDF_GetInfoAttr(hpdf, type);
    }
    to
    public string GetInfoAttr(HPdfInfoType type) {
        return Marshal.PtrToStringAnsi(HPDF_GetInfoAttr(hpdf, type));
    }
```
```
    public string GetFontName() {
        return HPDF_Font_GetFontName(hfont);
    }
    to
    public string GetFontName() {
        return Marshal.PtrToStringAnsi(HPDF_Font_GetFontName(hfont));
    }
```
```
    public string GetEncodingName() {
        HPDF_Font_GetEncodingName(hfont)
    }
    to
    public string GetEncodingName() {
        return Marshal.PtrToStringAnsi(HPDF_Font_GetEncodingName(hfont));
    }
```
```
    public string GetColorSpace() {
        return HPDF_Image_GetColorSpace(hobj);
    }
    to
    public string GetColorSpace() {
        return Marshal.PtrToStringAnsi(HPDF_Image_GetColorSpace(hobj));
    }
```
 
## Edit your C# code
As an example, this repository uses modified "JPFontDemo.cs" in the project "libharu_app.<br>
You can debug your C# code and libharu C code.

Completed! 😄
