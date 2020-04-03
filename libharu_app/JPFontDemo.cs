/*
 * << Haru Free PDF Library 2.0.5 >> -- JPFontDemo.cs
 *
 * Copyright (c) 1999-2006 Takeshi Kanno <takeshi_kanno@est.hi-ho.ne.jp>
 *
 * Permission to use, copy, modify, distribute and sell this software
 * and its documentation for any purpose is hereby granted without fee,
 * provided that the above copyright notice appear in all copies and
 * that both that copyright notice and this permission notice appear
 * in supporting documentation.
 * It is provided "as is" without express or implied warranty.
 *
 */

using System;
using System.IO;
using System.Drawing;
using HPdf;

public class JPFontDemo {
    public static void JPFontDemoMain() {

        Console.WriteLine("libhpdf-" + HPdfDoc.HPdfGetVersion());

        try {
            const int PAGE_HEIGHT = 210;
            string samp_text = "アメンボ赤いなあいうえお。浮き藻に小エビもおよいでる。";
            HPdfFont[] detail_font = new HPdfFont[16];

            HPdfDoc pdf = new HPdfDoc();

            /*declaration for using Japanese font, encoding. */
            pdf.UseJPEncodings();
            pdf.UseJPFonts();

            // フォントフォルダ取得
            string fontDir = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            Console.WriteLine(fontDir);

            // フォントファイル名は次のリンクを参考
            // https://helpx.adobe.com/jp/x-productkb/global/cq08041028.html
            //string myfontName = pdf.LoadTTFontFromFile2(fontDir + "\\msgothic.ttc", 0, false);
            string myfontName = pdf.LoadTTFontFromFile2(fontDir + "\\msmincho.ttc", 0, true);
            HPdfFont myfont = pdf.GetFont(myfontName, "90msp-RKSJ-H");

            detail_font[0] = pdf.GetFont("MS-Mincho", "90ms-RKSJ-H");
            detail_font[1] = pdf.GetFont("MS-Mincho,Bold", "90ms-RKSJ-H");
            detail_font[2] = pdf.GetFont("MS-Mincho,Italic", "90ms-RKSJ-H");
            detail_font[3] = pdf.GetFont("MS-Mincho,BoldItalic", "90ms-RKSJ-H");
            detail_font[4] = pdf.GetFont("MS-Mincho", "90msp-RKSJ-H");
            detail_font[5] = pdf.GetFont("MS-Mincho,Bold", "90msp-RKSJ-H");
            detail_font[6] = pdf.GetFont("MS-Mincho,Italic", "90msp-RKSJ-H");
            detail_font[7] = pdf.GetFont("MS-Mincho,BoldItalic",
            "90msp-RKSJ-H");
            detail_font[8] = pdf.GetFont("MS-Gothic", "90ms-RKSJ-H");
            detail_font[9] = pdf.GetFont("MS-Gothic,Bold", "90ms-RKSJ-H");
            detail_font[10] = pdf.GetFont("MS-Gothic,Italic", "90ms-RKSJ-H");
            detail_font[11] = pdf.GetFont("MS-Gothic,BoldItalic", "90ms-RKSJ-H");
            detail_font[12] = pdf.GetFont("MS-PGothic", "90msp-RKSJ-H");
            detail_font[13] = pdf.GetFont("MS-PGothic,Bold", "90msp-RKSJ-H");
            detail_font[14] = pdf.GetFont("MS-PGothic,Italic", "90msp-RKSJ-H");
            detail_font[15] = pdf.GetFont("MS-PGothic,BoldItalic",
            "90msp-RKSJ-H");

            /*configure pdf-document to be compressed. */
            pdf.SetCompressionMode(HPdfDoc.HPDF_COMP_ALL);

            /*Set page mode to use outlines. */
            pdf.SetPageMode(HPdfPageMode.HPDF_PAGE_MODE_USE_OUTLINE);

            /*create outline root. */
            HPdfOutline root = pdf.CreateOutline(null, "JP font demo", null);
            root.SetOpened (true);

            for (int i = 0; i <= 15; i++) {
                float x_pos;

                /*add a new page object. */
                HPdfPage page = pdf.AddPage();

                /*create outline entry */
                string tmp = detail_font[i].GetEncodingName();

                HPdfOutline outline = pdf.CreateOutline(root,
                        detail_font[i].GetFontName(), null);
                HPdfDestination dst = page.CreateDestination();
                outline.SetDestination(dst);

                //HPdfFont title_font = pdf.GetFont("Helvetica", null);
                HPdfFont title_font = pdf.GetFont(myfontName, "90msp-RKSJ-H");
                page.SetFontAndSize(title_font, 10);

                page.BeginText();

                /*move the position of the text to top of the page. */
                page.MoveTextPos(10, 190);
                page.ShowText(detail_font[i].GetFontName());

                page.SetFontAndSize(myfont, 15);
                page.MoveTextPos(100, 0);
                page.ShowText(samp_text);

                page.SetFontAndSize(detail_font[i], 15);
                page.MoveTextPos(10 - 100, -20);
                page.ShowText("abcdefghijklmnopqrstuvwxyz");
                page.MoveTextPos(0, -20);
                page.ShowText("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                page.MoveTextPos(0, -20);
                page.ShowText("1234567890");
                page.MoveTextPos(0, -20);

                page.SetFontAndSize(detail_font[i], 10);
                page.ShowText(samp_text);
                page.MoveTextPos(0, -18);

                page.SetFontAndSize(detail_font[i], 16);
                page.ShowText(samp_text);
                page.MoveTextPos(0, -27);

                page.SetFontAndSize(detail_font[i], 23);
                page.ShowText(samp_text);
                page.MoveTextPos(0, -36);

                page.SetFontAndSize(detail_font[i], 30);
                page.ShowText(samp_text);

                HPdfPoint p = page.GetCurrentTextPos();

                /*finish to print text. */
                page.EndText();

                page.SetLineWidth(0.5f);

                x_pos = 20;
                for (int j = 0; j <= samp_text.Length / 2; j++) {
                    page.MoveTo(x_pos, p.y - 10);
                    page.LineTo(x_pos, p.y - 12);
                    page.Stroke();
                    x_pos = x_pos + 30;
                }

                page.SetWidth(p.x + 20);
                page.SetHeight(PAGE_HEIGHT);

                page.MoveTo(10, PAGE_HEIGHT - 25);
                page.LineTo(p.x + 10, PAGE_HEIGHT - 25);
                page.Stroke();

                page.MoveTo(10, PAGE_HEIGHT - 85);
                page.LineTo(p.x + 10, PAGE_HEIGHT - 85);
                page.Stroke();

                page.MoveTo(10, p.y - 12);
                page.LineTo(p.x + 10, p.y - 12);
                page.Stroke();
            }

            pdf.SaveToFile("JPFontDemo.pdf");
        }
        catch (Exception e) {
            Console.Error.WriteLine(e.Message);
        }
    }
}



