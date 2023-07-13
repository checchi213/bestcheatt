namespace Vars
{
	namespace Global
	{
		Vector2 MenuPos = { 0, 0 };
		bool Panic = false;
		int ScreenHigh = 0;
		int ScreenWidth = 0;
		HWND hWindow = nullptr;
		bool MenuVisible = false;
		WNDPROC oWindproc = nullptr;

		Vector3 LastPos = { 0.f, 0.f, 0.f };
		ULONGLONG LastTime = 0;
		DWORD64 LastSteamID = 0;
		const wchar_t* LastName;
		Vector3 PlayerVeloc = { 0.f, 0.f, 0.f };

		int BoneToAim = 1;
		float BulletSpeed = 250.f;
		DWORD64 PresentDelta = 0;
		float CorX = 0.f;
		float CorY = 0.f;
	}

	namespace AimBot
	{
		bool Activate = false;
		bool BodyAim = false;
		bool RCS = false;
		bool Standalone = false;
		bool VisibleCheck = false;
		bool DrawFov = false;
		bool RightMouseAiming = false;
		bool IgnoreTeam = false;
		float Range = 200.f;
		float Fov = 10.f;
	}

	namespace Visuals
	{
		bool ShowPlayerBox = false;
		bool ShowPlayerName = false;
		bool ShowPlayerHealth = false;
		bool ShowPlayerWeapon = false;
		bool ShowPlayerDist = false;
		bool AlwaysDay = false;
		bool IgnoreSleepers = false;
		bool SkeletonPlayer = false;
		bool SkeletonBots = false;
		bool Crosshair = false;
		//
		bool PlayerESP = false;
		bool BotsESP = false;
	}

	namespace Misc
	{
		float pred = 0.f;
		bool SpiderMan = false;
		bool AntiSpread = false;
		bool NoRecoil = false;
		bool FastReload = false;
		bool Automatic = false;
		bool SuperEoka = false;
		bool SkinChanger = false;
		bool BuildAsUWant = false;
		bool NoSway = false;		
		bool SuperBow = false;
		bool FakeAdmin = false; //need
		bool NoGreenZone = false;
		bool LongHand = false;
		bool FatHand = false;
		bool RunOnHit = false;
		bool HighJump = false;
		float JumpValue = 1.0f;
		bool WaterBoost = false;
		bool NoFall = false;
		bool AirStuck = false;
		bool AntiKastrulya = false;
		bool NoHeavyReduct = false;				
		//meme dot
		bool Crosshair = false;
	}
}