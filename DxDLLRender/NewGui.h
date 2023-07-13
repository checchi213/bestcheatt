#include <d2d1.h>
#include <dwrite_1.h>
#include <intrin.h>
#pragma comment(lib, "d2d1.lib")
#pragma comment(lib, "dwrite.lib")

class RenderClass
{
private:
	//render context
	ID2D1Factory* Interface;
	ID2D1RenderTarget* Canvas;
	IDWriteFactory1* TextEngine;
	IDWriteTextFormat* TextFormat;
	ID2D1SolidColorBrush* SolidColor;

	//fast get wstring length
	__forceinline UINT wcslen(const wchar_t* Str) {
		const wchar_t *TempStr = Str;
		for (; *TempStr; ++TempStr);
		return (UINT)(TempStr - Str);
	}

	__declspec(noinline) bool InitRender(IDXGISwapChain* SwapChain)
	{
		//prep d2d render & text engine
		static bool initialized; if (!initialized) {
			initialized = true; D2D1_FACTORY_OPTIONS CreateOpt = { D2D1_DEBUG_LEVEL_NONE };
			FC(dwrite, DWriteCreateFactory, DWRITE_FACTORY_TYPE_SHARED, __uuidof(TextEngine), (IUnknown**)&TextEngine);
			FC(d2d1, D2D1CreateFactory, D2D1_FACTORY_TYPE_SINGLE_THREADED, __uuidof(ID2D1Factory), &CreateOpt, (void**)&Interface);
			TextEngine->CreateTextFormat(StrW(L"Verdana"), NULL, DWRITE_FONT_WEIGHT_THIN, DWRITE_FONT_STYLE_NORMAL, DWRITE_FONT_STRETCH_CONDENSED, 12.f, L"", &TextFormat);
			if (!Interface || !TextEngine || !TextFormat) return false;
		}

		//get window flags var
		ID3D11Device* d3d_device;
		if (SwapChain->GetDevice(IID_PPV_ARGS(&d3d_device))) return false;
		WORD flagsOffset = *(WORD*)((*(DWORD64**)d3d_device)[38] + 2); //x64
		int& flags = *(INT*)((DWORD64)d3d_device + flagsOffset);
		d3d_device->Release();

		//get d3d backbuffer
		IDXGISurface* d3d_bbuf;
		if (SwapChain->GetBuffer(0, IID_PPV_ARGS(&d3d_bbuf)))
			return false;

		//create canvas & cleanup
		D2D1_RENDER_TARGET_PROPERTIES d2d_prop = D2D1::RenderTargetProperties(D2D1_RENDER_TARGET_TYPE_HARDWARE, D2D1::PixelFormat(DXGI_FORMAT_UNKNOWN, D2D1_ALPHA_MODE_PREMULTIPLIED));
		flags |= 0x20; HRESULT canvas_state = Interface->CreateDxgiSurfaceRenderTarget(d3d_bbuf, d2d_prop, &Canvas); flags &= ~0x20; d3d_bbuf->Release(); if (canvas_state) return false;
		if (!SolidColor) Canvas->CreateSolidColorBrush({}, &SolidColor); return true;
	}

public:
	//canvas mgr
	__forceinline bool NewFrame(IDXGISwapChain* SwapChain)
	{
		//need prep d2d canvas
		if (!Canvas && !InitRender(SwapChain)) 
			return false;

		//draw start
		Canvas->BeginDraw();
		return true;
	}
	
	__forceinline Vector2 CanvasSize() {
		D2D1_SIZE_F Size = Canvas->GetSize();
		return Vector2{ Size.width, Size.height };
	}

	__forceinline void ResetCanvas() {
		if (Canvas) {
			Canvas->Release();
			Canvas = nullptr;
		}
	}

	__forceinline void EndFrame() {
		HRESULT state = Canvas->EndDraw();
		if (state == D2DERR_RECREATE_TARGET) 
			ResetCanvas();
	}

	//line
	__forceinline void Line(const Vector2& Start, const Vector2& End, const D2D1::ColorF& Clr, float Thick = 1.5f) {
		SolidColor->SetColor(Clr); Canvas->DrawLine({ Start.x, Start.y }, { End.x, End.y }, SolidColor, Thick);
	}

	//circle
	__forceinline void Сircle(const Vector2& Start, const D2D1::ColorF& Clr, float Rad, float Thick = 1.5f) {
		SolidColor->SetColor(Clr); Canvas->DrawEllipse({ { Start.x, Start.y }, Rad, Rad }, SolidColor, Thick);
	}

	__forceinline void FillCircle(const Vector2& Start, const D2D1::ColorF& Clr, float Rad) {
		SolidColor->SetColor(Clr); Canvas->FillEllipse({ { Start.x, Start.y }, Rad, Rad }, SolidColor);
	}

	//rectangle
	__forceinline void Rectangle(const Vector2& Start, const Vector2& Sz, const D2D1::ColorF& Clr, float Thick = 1.5f) {
		SolidColor->SetColor(Clr); Canvas->DrawRectangle({ Start.x, Start.y, Start.x + Sz.x, Start.y + Sz.y }, SolidColor, Thick);
	}

	__forceinline void FillRectangle(const Vector2& Start, const Vector2& Sz, const D2D1::ColorF& Clr) {
		SolidColor->SetColor(Clr); Canvas->FillRectangle({ Start.x, Start.y, Start.x + Sz.x, Start.y + Sz.y }, SolidColor);
	}

	__forceinline void RoundedRectangle(const Vector2& Start, const Vector2& Sz, const D2D1::ColorF& Clr, float Rad, float Thick = 1.5f) {
		SolidColor->SetColor(Clr); Canvas->DrawRoundedRectangle({ {Start.x, Start.y, Start.x + Sz.x, Start.y + Sz.y }, Rad, Rad }, SolidColor, Thick);
	}

	__forceinline void FillRoundedRectangle(const Vector2& Start, const Vector2& Sz, const D2D1::ColorF& Clr, float Rad) {
		SolidColor->SetColor(Clr); Canvas->FillRoundedRectangle({ {Start.x, Start.y, Start.x + Sz.x, Start.y + Sz.y}, Rad, Rad }, SolidColor);
	}
	
	//text
	__forceinline Vector2 StringCenter(const Vector2& Start, const wchar_t* Str, const D2D1::ColorF& Clr = D2D1::ColorF(D2D1::ColorF::White)) {
		SolidColor->SetColor(Clr); IDWriteTextLayout* TextLayout; TextEngine->CreateTextLayout(Str, this->wcslen(Str), TextFormat, 200.f, 100.f, &TextLayout);
		DWRITE_TEXT_METRICS TextInfo; TextLayout->GetMetrics(&TextInfo); Vector2 TextSize = { TextInfo.width / 2.f, TextInfo.height / 2.f };
		Canvas->DrawTextLayout({ Start.x - TextSize.x, Start.y - TextSize.y }, TextLayout, SolidColor); TextLayout->Release();
		return TextSize; //return half text size
	}

	__forceinline void String(const Vector2& Start, const wchar_t* Str, const D2D1::ColorF& Clr = D2D1::ColorF(D2D1::ColorF::White)) {
		SolidColor->SetColor(Clr); Canvas->DrawTextW(Str, this->wcslen(Str), TextFormat, { Start.x, Start.y, FLT_MAX, FLT_MAX }, SolidColor);
	}	
};

namespace GUI
{
	bool Visible;
	RenderClass Render;
	WNDPROC NextWndProc;
	enum Button {
		NoPress,
		Clicked,
		Pressed
	};
	struct IO {
		Button LKM;
		Vector2 MousePos;
		Vector2 MouseDelta;
		Vector2 OldMousePos;
		USHORT CurElement;
	} IO;
	struct WndData {
		Vector2 WndPos;
		Vector2 Size;
		Vector2 Pos;
	} CurWnd;

	//str hash
	unsigned short __forceinline HashStr(const wchar_t* Str)
	{
		unsigned char i;
		unsigned short crc = 0xFFFF;
		while (wchar_t DChar = *Str++) {
			unsigned char Char = (unsigned char)DChar;
			crc ^= Char << 8;
			for (i = 0; i < 8; i++)
				crc = crc & 0x8000 ? (crc << 1) ^ 0x1021 : crc << 1;
			Char = (unsigned char)(DChar >> 8);
			crc ^= Char << 8;
			for (i = 0; i < 8; i++)
				crc = crc & 0x8000 ? (crc << 1) ^ 0x1021 : crc << 1;
		} return crc;
	}

	Vector2 __forceinline CenterLine(const Vector2& Pos) {
		return { (Pos.x + (CurWnd.Size.x / 2)), Pos.y };
	}

	bool __forceinline InRect(Vector2 Rect, Vector2 Size, Vector2 Dot) {
		return (Dot.x > Rect.x && Dot.x < Rect.x + Size.x && Dot.y > Rect.y && Dot.y < Rect.y + Size.y);
	}

	//input
	LRESULT WINAPI WndProc(HWND Wnd, UINT Msg, WPARAM wParam, LPARAM lParam)
	{
		switch (Msg)
		{
		case WM_LBUTTONDOWN:
			IO.LKM = Pressed;
			break;

		case WM_LBUTTONUP:
			IO.LKM = Clicked;
			IO.CurElement = 0;
			break;

		case WM_KEYUP:
			if (wParam == VK_INSERT)
				Visible = !Visible;
			break;

		case WM_MOUSEMOVE:
			IO.MousePos.x = (signed short)(lParam);
			IO.MousePos.y = (signed short)(lParam >> 16);
			break;
		}

		return FC(user32, CallWindowProcW, NextWndProc, Wnd, Msg, wParam, lParam);
	}

	void ProcessInput(bool End = false)
	{
		if (!End)
		{
			//calc mouse delta
			IO.MouseDelta = IO.MousePos - IO.OldMousePos;
			IO.OldMousePos = IO.MousePos;
		}

		else
		{
			//update LKM button
			if (IO.LKM == Clicked)
				IO.LKM = NoPress;
		}
	}

	bool InputMgr(const wchar_t* Name, bool Reg = false) {
		unsigned short StrHash = HashStr(Name);
		if (Reg && !IO.CurElement) {
			IO.CurElement = StrHash;
			return true;
		}
		else if (!Reg)
			return (IO.CurElement == StrHash);
		return false;
	}

	//main
	void SliderFloat(const wchar_t* Name, float* Current, float Min, float Max)
	{
		Render.String({ CurWnd.Pos.x - 136 + 10.f, CurWnd.Pos.y + 50}, FC_FORMAT_W(L"%s : %d", Name, (int)*Current));
		CurWnd.Pos.y += 20.f;

		if (IO.LKM == Pressed && InRect({ CurWnd.Pos.x - 136 + 10.f, CurWnd.Pos.y + 50 }, { CurWnd.Size.x - 280 - 20.f, 14.f }, IO.MousePos)) {
			float Val = ((((IO.MousePos.x + 118 - CurWnd.Pos.x) / (CurWnd.Size.x - 280 - 36.f)) * (Max - Min)) + Min);
			*Current = ((Val > Max) ? Max : ((Val < Min) ? Min : Val)); InputMgr(Name, true);
		}

		float CurOff = (*Current - Min) / (Max - Min);
		Render.FillRoundedRectangle({ CurWnd.Pos.x - 136 + 10.f, CurWnd.Pos.y + 50 }, { CurWnd.Size.x - 280 - 20.f, 11.f }, D2D1::ColorF{ 3 / 255.f, 19 / 255.f, 35 / 255.f }, 8.f);

		Render.FillRoundedRectangle( { CurWnd.Pos.x - 136 + 10.f, CurWnd.Pos.y + 50 }, { (CurWnd.Size.x - 280 - 20.f)  * CurOff, 11.f }, D2D1::ColorF{ 5/255.f, 146 / 255.f, 183 / 255.f }, 8.f);

		Render.FillCircle({ CurWnd.Pos.x - 136 + 18.f + ((CurWnd.Size.x - 280 - 36.f) * CurOff), CurWnd.Pos.y + 50 + 6 }, D2D1::ColorF::White, 8.f);
		CurWnd.Pos.y += 24.f;
	}

	void SliderFloatS(const wchar_t* Name, float* Current, float Min, float Max)
	{
		float adapt = Max / 100; int AdaptCurrent = *Current / adapt;
		Render.String({ CurWnd.Pos.x - 136 + 10.f, CurWnd.Pos.y + 50 }, FC_FORMAT_W(L"%s : %d", Name, AdaptCurrent));
		CurWnd.Pos.y += 20.f;

		if (IO.LKM == Pressed && InRect({ CurWnd.Pos.x - 136 + 10.f, CurWnd.Pos.y + 50 }, { CurWnd.Size.x - 280 - 20.f, 14.f }, IO.MousePos)) {
			float Val = ((((IO.MousePos.x + 118 - CurWnd.Pos.x) / (CurWnd.Size.x - 280 - 36.f)) * (Max - Min)) + Min);
			*Current = ((Val > Max) ? Max : ((Val < Min) ? Min : Val)); InputMgr(Name, true);
		}

		float CurOff = (*Current - Min) / (Max - Min);
		Render.FillRoundedRectangle({ CurWnd.Pos.x - 136 + 10.f, CurWnd.Pos.y + 50 }, { CurWnd.Size.x - 280 - 20.f, 11.f }, D2D1::ColorF{ 3 / 255.f, 19 / 255.f, 35 / 255.f }, 8.f);

		Render.FillRoundedRectangle({ CurWnd.Pos.x - 136 + 10.f, CurWnd.Pos.y + 50 }, { (CurWnd.Size.x - 280 - 20.f)  * CurOff, 11.f }, D2D1::ColorF{ 5 / 255.f, 146 / 255.f, 183 / 255.f }, 8.f);

		Render.FillCircle({ CurWnd.Pos.x - 136 + 18.f + ((CurWnd.Size.x - 280 - 36.f) * CurOff), CurWnd.Pos.y + 50 + 6 }, D2D1::ColorF::White, 8.f);
		CurWnd.Pos.y += 24.f;
	}

	void CheckBox(const wchar_t* Name, bool* Switch)
	{
		bool SW = *Switch; Render.String({ CurWnd.Pos.x - 125, CurWnd.Pos.y + 50 }, (wchar_t*)Name);
		if (IO.LKM == Clicked && InRect({ CurWnd.Pos.x + 30, CurWnd.Pos.y + 50 }, { 38.f, 14.f }, IO.MousePos) && InputMgr(Name, true)) *Switch = SW = !SW;
		Render.FillRoundedRectangle({ CurWnd.Pos.x + 30, CurWnd.Pos.y + 50 }, { 38.f, 14.f }, D2D1::ColorF(3 / 255.f, 19 / 255.f, 35 / 255.f), 8.f);
		Render.FillCircle({ CurWnd.Pos.x + 38.f + (SW ? 21.f : 0.f), CurWnd.Pos.y + 57.f }, SW ? D2D1::ColorF(5/ 255.f, 146 / 255.f, 183 / 255.f) : D2D1::ColorF(199 / 255.f, 199 / 255.f, 197 / 255.f), 9.f);
		CurWnd.Pos.y += 20.f;
	}
	static int page = 1;
	void ButtonUi(const wchar_t* Name , int tabnum)
	{
		if (IO.LKM == Clicked && InRect({ CurWnd.Pos.x + 140.f, CurWnd.Pos.y - 2 }, { 40.f, 16.f }, IO.MousePos) && InputMgr(Name, true)) page = tabnum;
		Render.String({ CurWnd.Pos.x + 140.f, CurWnd.Pos.y - 2 }, (wchar_t*)Name, page == tabnum ? D2D1::ColorF::White : D2D1::ColorF(34/255.f, 136 / 255.f, 159 / 255.f));
		CurWnd.Pos.x += 55.f;
	}

	void Spacing(const wchar_t* Name) {
		Vector2 CntLine = CenterLine(CurWnd.Pos); Vector2 TextSize = Render.StringCenter(CntLine, Name);
		Render.Line({ CntLine.x - TextSize.x - 2.f, CntLine.y }, { CurWnd.Pos.x + 4.f, CurWnd.Pos.y }, D2D1::ColorF::White);
		Render.Line({ CntLine.x + TextSize.x + 2.f, CntLine.y }, { CurWnd.Pos.x + CurWnd.Size.x - 4.f, CurWnd.Pos.y }, D2D1::ColorF::White);
		CurWnd.Pos.y += 8.f;
	}

	void Begin(const wchar_t* Name, Vector2& Pos, const Vector2& Size, const D2D1::ColorF& Clr)
	{
		//base menu
		ProcessInput();
		if (!CurWnd.WndPos.Zero()) Pos = CurWnd.WndPos;

		const wchar_t* cheat_name = L"GXONE Multihack";
		const wchar_t* bottom_text = L"gxone recode " "[" __DATE__ "]";
		const wchar_t* bottom_text2 = L"gxone.net \u00A9";
		const wchar_t* crosshair = L"\u2316";

		Render.FillRectangle(Pos, { 620 ,500 }, D2D1::ColorF(17 / 255.f, 33 / 255.f, 49 / 255.f));
		Render.FillRectangle(Pos, Vector2(620, 60), D2D1::ColorF(3 / 255.f, 15 / 255.f, 27 / 255.f));
		Render.FillRectangle(Vector2(Pos.x, Pos.y + 60), Vector2(620, 440), D2D1::ColorF(16/255.f, 63 / 255.f, 84 / 255.f));
		Render.FillRectangle(Vector2(Pos.x, Pos.y + 480), Vector2(620, 20), D2D1::ColorF(3 / 255.f, 15 / 255.f, 27 / 255.f));
		Render.FillRectangle(Vector2(Pos.x + 90, Pos.y + 70), { 520 , 400 }, D2D1::ColorF(17 / 255.f, 33 / 255.f, 49 / 255.f , 0.7));

		Render.String(Vector2(Pos.x + 15, Pos.y + 23) , cheat_name, D2D1::ColorF(1.f,1.f,1.f));
		Render.String(Vector2(Pos.x + 5, Pos.y + 482), bottom_text, D2D1::ColorF(1.f, 1.f, 1.f));
		Render.String(Vector2(Pos.x + 539, Pos.y + 482), bottom_text2, D2D1::ColorF(1.f, 1.f, 1.f));
		
		// а тут пошел говнокод , ну и чо ? 
		if (page == 1) // rage
		{
			Render.String(Vector2(Pos.x + 5, Pos.y + 70), L"ragebot tab", D2D1::ColorF(14/255.f, 163 / 255.f, 238 / 255.f));
			Render.String(Vector2(Pos.x + 5, Pos.y + 85), L"ragebot", D2D1::ColorF(1.f, 1.f, 1.f));
		}
		else if (page == 2) //legit
		{
			Render.String(Vector2(Pos.x + 5, Pos.y + 70), L"weapon tab", D2D1::ColorF(14 / 255.f, 163 / 255.f, 238 / 255.f));
			Render.String(Vector2(Pos.x + 5, Pos.y + 85), L"weapon", D2D1::ColorF(1.f, 1.f, 1.f));
		}
		else if (page == 3) // visuals
		{
			Render.String(Vector2(Pos.x + 5, Pos.y + 70), L"visuals tab", D2D1::ColorF(14 / 255.f, 163 / 255.f, 238 / 255.f));
			Render.String(Vector2(Pos.x + 5, Pos.y + 85), L"visuals", D2D1::ColorF(1.f, 1.f, 1.f));
		}
		else if (page == 4) // misc
		{
			Render.String(Vector2(Pos.x + 5, Pos.y + 70), L"misc tab", D2D1::ColorF(14 / 255.f, 163 / 255.f, 238 / 255.f));
			Render.String(Vector2(Pos.x + 5, Pos.y + 85), L"misc", D2D1::ColorF(1.f, 1.f, 1.f));
		}

		CurWnd.WndPos = { Pos.x, Pos.y }; CurWnd.Size = Size;
		CurWnd.Pos = { Pos.x, Pos.y + 25.f };
	}
	
	void End() 
	{
		//drag window
		ProcessInput(true);
		if (InputMgr(L"##Drag") || (IO.LKM == Pressed && InRect(CurWnd.WndPos, CurWnd.Size, IO.MousePos) && InputMgr(L"##Drag", true)))
			CurWnd.WndPos += IO.MouseDelta;
	}
}