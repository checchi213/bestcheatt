#pragma once
typedef void(__stdcall* Labell)(Rect, DWORD64, DWORD64);
typedef void(__stdcall* Aligment)(DWORD64, DWORD64);
typedef void(__stdcall* SetClr)(Color);
typedef int(__stdcall* EventT)(DWORD64);
typedef DWORD64(__stdcall* RetEmpty)();
typedef void(__stdcall* SetText)(DWORD64, int);
typedef DWORD64(__stdcall* Content)(Str*);
typedef void(__stdcall* DrawT)(Rect, DWORD64);

DWORD64 g_WhiteTexture = 0;
//DWORD64 oEvent = 0; DWORD64 oEventKeyCode = 0; DWORD64 oEventType = 0; DWORD64 oMousePos = 0;
float g_GUICalled = 0.f;
namespace GUI {
	DWORD64 label = 0; DWORD64 skin = 0;

	void Init() {

	}

	void Prepare(int size = 12, int alig = 0x0 /*top left*/, Color clr = Color(1, 1, 1, 1)) {
		if (label && skin) return;
		g_WhiteTexture = ((RetEmpty)(g_Base + O::UnityEngine_Texture2D::get_whiteTexture))();
		typedef DWORD64(__stdcall* RetEmpty)();
		RetEmpty get_skin = (RetEmpty)(g_Base + O::UnityEngine_GUI::get_skin);
		SetText set_textsize = (SetText)(g_Base + O::UnityEngine_GUIStyle::set_fontSize);
		SetClr set_clr = (SetClr)(g_Base + O::UnityEngine_GUI::set_color);

		GUI::skin = get_skin();
		GUI::label = safe_read(skin + 0x38, DWORD64);

		((Aligment)(g_Base + O::UnityEngine_GUIStyle::set_alignment))(label, alig);

		set_textsize(label, size);          //prepare style
		set_clr(clr);
	}
	void Label(Rect pos, Str text, Color clr = Color(1, 1, 1, 1), bool centered = false, float size = 12.f) {
		SetClr set_clr = (SetClr)(g_Base + O::UnityEngine_GUI::set_color);
		Labell draw_label = (Labell)(g_Base + S::Label);
		Content content_create = (Content)(g_Base + O::UnityEngine_GUIContent::Temp);
		SetText set_textsize = (SetText)(g_Base + O::UnityEngine_GUIStyle::set_fontSize);
		Aligment set_alig = (Aligment)(g_Base + O::UnityEngine_GUIStyle::set_alignment);
		set_clr(clr);

		DWORD64 content = content_create(&text);

		set_textsize(label, size);          //prepare style

		if (centered) set_alig(label, 0x4);
		else set_alig(label, 0x0);

		draw_label(pos, content, label);
	}
	void FillBox(Rect pos, Color clr) {
		
		SetClr set_clr = (SetClr)(g_Base + O::UnityEngine_GUI::set_color);
		set_clr(clr);
		DrawT DrawTexture = (DrawT)(g_Base + O::UnityEngine_GUI::DrawTexture);
		DrawTexture(pos, g_WhiteTexture);
	}

	void VerticalLine(Vector2 start, float down, Color clr, float wid = 1.f) {
		return FillBox(Rect(start.x, start.y, wid, down), clr);
	}
	void HorizontalLine(Vector2 start, float right, Color clr, float wid = 1.f) {
		return FillBox(Rect(start.x, start.y, right, wid), clr);
	}
	void Box(Vector2 pos, Vector2 size, Color clr, float wid = 1.f) {
		SetClr set_clr = (SetClr)(g_Base + O::UnityEngine_GUI::set_color); //public class GUI
		set_clr(clr);
		DrawT DrawTexture = (DrawT)(g_Base + O::UnityEngine_GUI::DrawTexture);
		DrawTexture(Rect(pos.x, pos.y, wid, size.y), g_WhiteTexture);
		DrawTexture(Rect(pos.x + size.x, pos.y, wid, size.y), g_WhiteTexture);
		DrawTexture(Rect(pos.x, pos.y, size.x, wid), g_WhiteTexture);
		DrawTexture(Rect(pos.x, pos.y + size.y, size.x, wid), g_WhiteTexture);
	}
};
void ESPLoop();
void DrawCrosshair();
void MenuTick(DWORD64 Event, EventType type);

DWORD64 retMainTick = 0;
void MainTick(DWORD64 unk) {
	//MessageBoxA(NULL, "YO", "YO", 0);
	if(LocalPlayer) g_GUICalled = LocalPlayer->Time();
	GUI::Prepare();
	
	DWORD64 Event = ((RetEmpty)(g_Base + O::UnityEngine_Event::get_current))();
	EventType type = (EventType)((EventT)(g_Base + O::UnityEngine_Event::get_type))(Event);

	if (type == EventType::Repaint) {
		if (!Vars::User::Panic && Vars::PlayerEsp::Logo) {
			GUI::Label(Rect(0, 0, 400, 100), Str(xorstr(L"SPERMA WARE")), Color(1, 1, 1, 1), false, 14.f);
		}
		

		if (!Vars::User::Panic) {
			ESPLoop();
			DrawCrosshair();
		}
	}

	if (!Vars::User::Panic) {
		LocalPlayer->SetFlag(4); //for noclip & debugcamera
	}
	MenuTick(Event, type);
}