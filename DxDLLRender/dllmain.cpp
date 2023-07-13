#pragma warning(disable : 4530)
#define StrA
#define StrW
#include <Windows.h>
#include <psapi.h>
#include <d3d11.h>
#include <emmintrin.h>
#include <winternl.h>
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <float.h>

#include "sdk/NewUtils.h"
#include "sdk/utils.h"
#include "sdk/safe_mem.h"
#include "sdk_new/math.h"
#include "vars/vars.h"
#include "sdk/game_sdk.h"
#include "NewGui.h"

#include "cheat_funcs/EspFuncs.h"
#include "cheat_funcs/AimFuncs.h"
#include "cheat_funcs/MiscFuncs.h"
#include "cheat_funcs/EntityZaLoop.h"

//#include "FLIC.h"

//discord depends
UINT vps = 1;
D3D11_VIEWPORT viewport;
ID3D11Device *pDevice = NULL;
ID3D11DeviceContext *pContext = NULL;
ID3D11RenderTargetView* RenderTargetView = NULL;

#define AllocCons 0
bool panic = false;

typedef HRESULT(__fastcall* DX11PresentFn) (IDXGISwapChain* pSwapChain, UINT syncintreval, UINT flags);
DX11PresentFn oPresent = nullptr;

typedef HRESULT(__stdcall* DX11ResizeFn)(IDXGISwapChain*, UINT, UINT, UINT, DXGI_FORMAT, UINT);
DX11ResizeFn oResize = nullptr;

HRESULT __stdcall Resize(IDXGISwapChain* Swap, UINT a2, UINT a3, UINT a4, DXGI_FORMAT a5, UINT a6) { 
	GUI::Render.ResetCanvas(); return oResize(Swap, a2, a3, a4, a5, a6); 
}

HRESULT __stdcall hookD3D11Present(IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags)
{
	if (GUI::Render.NewFrame(pSwapChain))
	{
		
		if (!Vars::Global::Panic)
		{
			float Y = GetSystemMetrics(SM_CYSCREEN);
			
			Vector2 kek = GUI::Render.CanvasSize();
			Vars::Global::ScreenWidth = kek.x;
			Vars::Global::ScreenHigh = kek.y;
		
			
			if (Vars::AimBot::BodyAim) Vars::Global::BoneToAim = 21;
			else Vars::Global::BoneToAim = 46;

			if (!LocalPlayer.BasePlayer->IsMenu())
			{
				if (Vars::Visuals::Crosshair)
					Crosshair();

				if (Vars::AimBot::DrawFov)
					DrawFOV();
			}
			
			GUI::Render.String({ 10.f, 10.f }, L"GX_1_Recode", D2D1::ColorF::Purple);


			EntityZaLoop();

				if (GUI::Visible && LocalPlayer.BasePlayer->IsMenu()) {

				if (Vars::AimBot::Fov > (kek.y - 3)) Vars::AimBot::Fov = (kek.y - 3);

				static int curtab = 0;
				GUI::Begin(L"GX1", Vars::Global::MenuPos, Vector2{ 741, 670 }, { 0, 0, 0, 1 });
				GUI::ButtonUi(L"aimbot" , 1);
				GUI::ButtonUi(L"weapon" , 2);
				GUI::ButtonUi(L"visuals", 3);
				GUI::ButtonUi(L"misc", 4);

				if (GUI::page == 1)
				{
					GUI::CheckBox(L"enable", &Vars::AimBot::Activate);
					GUI::CheckBox(L"body Aim", &Vars::AimBot::BodyAim);
					GUI::CheckBox(L"recoil control system", &Vars::AimBot::RCS);
					GUI::CheckBox(L"RCS standalone", &Vars::AimBot::Standalone);
					GUI::CheckBox(L"right mouse aiming", &Vars::AimBot::RightMouseAiming);
					GUI::CheckBox(L"visible check", &Vars::AimBot::VisibleCheck);
					GUI::CheckBox(L"show FOV", &Vars::AimBot::DrawFov);
					GUI::CheckBox(L"ignore Team", &Vars::AimBot::IgnoreTeam);
					GUI::SliderFloat(L"fov", &Vars::AimBot::Fov, 0.f, ((kek.y / 2.f) - 3));
					GUI::SliderFloat(L"max Distance", &Vars::AimBot::Range, 0.f, 400.f);
				}
				if (GUI::page == 2)
				{
					GUI::CheckBox(L"no recoil", &Vars::Misc::NoRecoil);
					GUI::CheckBox(L"no spread", &Vars::Misc::AntiSpread);
					GUI::CheckBox(L"fast reload", &Vars::Misc::FastReload);
					GUI::CheckBox(L"automatic", &Vars::Misc::Automatic);
					GUI::CheckBox(L"no sway", &Vars::Misc::NoSway);
					//GUI::CheckBox(L"SkinChanger", &Vars::Misc::SkinChanger);
					GUI::CheckBox(L"100% EOKA", &Vars::Misc::SuperEoka);
					//GUI::CheckBox(L"Fast Bow", &Vars::Misc::SuperBow);
					GUI::CheckBox(L"super melee", &Vars::Misc::LongHand);
				}

				if (GUI::page == 3)
				{
					GUI::CheckBox(L"enable player ESP", &Vars::Visuals::PlayerESP);
					GUI::CheckBox(L"box", &Vars::Visuals::ShowPlayerBox);
					GUI::CheckBox(L"health", &Vars::Visuals::ShowPlayerHealth);
					GUI::CheckBox(L"weapon (unussed)", &Vars::Visuals::ShowPlayerWeapon);
					GUI::CheckBox(L"distance (unussed)", &Vars::Visuals::ShowPlayerDist);
					GUI::CheckBox(L"ignore sleepers", &Vars::Visuals::IgnoreSleepers);
					GUI::CheckBox(L"skeleton", &Vars::Visuals::SkeletonPlayer);
					GUI::CheckBox(L"enable bots ESP", &Vars::Visuals::BotsESP);
					GUI::CheckBox(L"enable bots skeleton", &Vars::Visuals::SkeletonBots);
				}

				if (GUI::page == 4)
				{
					GUI::CheckBox(L"fake admin", &Vars::Misc::FakeAdmin);
					GUI::CheckBox(L"crosshair", &Vars::Visuals::Crosshair);
					GUI::CheckBox(L"always day", &Vars::Visuals::AlwaysDay);
					GUI::CheckBox(L"spiderMan", &Vars::Misc::SpiderMan);
					//GUI::CheckBox(L"Fast Swim", &Vars::Misc::WaterBoost);
					//GUI::CheckBox(L"Fast Run", &Vars::Misc::NoHeavyReduct);
					GUI::CheckBox(L"gravity (Middle Mouse)", &Vars::Misc::HighJump);
					GUI::SliderFloatS(L"gravity percentage", &Vars::Misc::JumpValue, 0.f, 2.5f);
					GUI::CheckBox(L"freeze (Mouse 5)", &Vars::Misc::AirStuck);
					GUI::CheckBox(L"unlock aiming in heavy", &Vars::Misc::AntiKastrulya);
					GUI::CheckBox(L"roll buildings", &Vars::Misc::BuildAsUWant);
					GUI::CheckBox(L"unload cheat", &Vars::Global::Panic);
				}

				GUI::End();
			}
		}

		GUI::Render.EndFrame();
	}
	end:
	Vars::Global::PresentDelta = 0;
	return oPresent(pSwapChain, SyncInterval, Flags);
}

__declspec(noinline) void Flex2()
{
	Sleep(  0);
}


__declspec(noinline) void StartRender()
{
	DWORD64 dwX64PresentOrig = NULL;DWORD64 dwX64ResizeBuffers = NULL;
	HWND Wnd = FindWindowA(  StrA("UnityWndClass"), NULL);
	GUI::NextWndProc = (WNDPROC)SetWindowLongPtrW(  Wnd, GWLP_WNDPROC, (LONG_PTR)GUI::WndProc);

	PDWORD64 origPresent = (PDWORD64)RVA(FindPattern((PBYTE)"\xFF\x15\x00\x00\x00\x00\x8B\xD8\xE8\x00\x00\x00\x00\xE8\x00\x00\x00\x00\xEB\x10", StrA("xx????xxx????x????xx"), StrW(L"DiscordHook64.dll")), 6);
	dwX64PresentOrig = *origPresent;
	oPresent = (DX11PresentFn)(*origPresent);
	*origPresent = (DWORD_PTR)(&hookD3D11Present);

	PDWORD64 origResize = (PDWORD64)RVA(FindPattern((PBYTE)"\x44\x8B\xCB\x44\x8B\xC7", StrA("xxxxxx"), StrW(L"DiscordHook64.dll")) + 0x1F, 7);
	dwX64ResizeBuffers = *origResize;
	oResize = (DX11ResizeFn)(*origResize);
	*origResize = (DWORD_PTR)(&Resize);

	//CreateThreadSafe(EntityHandler);
	//CreateThreadSafe(CheatFunc);
}

BOOL __stdcall DllMain(HMODULE hModule, DWORD dwCallReason, LPVOID lpReserved)
{
	
	if (dwCallReason == DLL_PROCESS_ATTACH)
	{
#if AllocCons == 1
		FC(kernel32, AllocConsole);
		//FILE* pFILE = 0;
		//freopen_s(&pFILE, "CONIN$", "r", stdin);
		//freopen_s(&pFILE, "CONOUT$", "w", stdout);
		//freopen_s(&pFILE, "CONOUT$", "w", stderr);
#endif
		//FLIC::Start();

		StartRender();
	}

	return TRUE;
}
