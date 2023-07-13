void EntityZaLoop()
{
	static DWORD64 dwGameObjectManager = 0;
	if(!dwGameObjectManager)
		dwGameObjectManager = RVA(FindPattern((PBYTE)"\x48\x8B\x15\x00\x00\x00\x00\x66\x39", "xxx????xx", L"UnityPlayer.dll"), 7);
	
	DWORD64 ObjMgr = safe_read(dwGameObjectManager, DWORD64); if (!ObjMgr) return;

	//aim vars
	BasePlayer* AimEntity = nullptr;
	float FOV = Vars::AimBot::Fov, CurFOV;
	
		//entity list
	bool LP_isValid = false;
	for (DWORD64 Obj = safe_read(ObjMgr + 0x8, DWORD64); (Obj && (Obj != safe_read(ObjMgr, DWORD64))); Obj = safe_read(Obj + 0x8, DWORD64))
	{
		DWORD64 GameObject = safe_read(Obj + 0x10, DWORD64);
		WORD Tag = safe_read(GameObject + 0x54, WORD);

		if (Tag == 6 || Tag == 5 || Tag == 20011)
		{
			DWORD64 ObjClass = safe_read(GameObject + 0x30, DWORD64);
			DWORD64	Entity = safe_read(ObjClass + 0x18, DWORD64);

			//entity
			if (Tag == 6)
			{
				BasePlayer* Player = (BasePlayer*)safe_read(Entity + 0x28, DWORD64);
				if (!Player->IsValid()) continue;

				//entity
				if (safe_read(safe_read(GameObject + 0x60, DWORD64), DWORD64) != 0x616C506C61636F4C)
				{
					//exec esp
					ESP(Player, LocalPlayer.BasePlayer);

					//find target
					if (Vars::AimBot::IgnoreTeam) {
						DWORD64 EntitySID = Player->GetSteamID();
						for (int j = 0; j < LocalPlayer.BasePlayer->GetTeamSize(); j++) {
							DWORD64 SlotSID = LocalPlayer.BasePlayer->IsTeamMate(j);
							if (SlotSID == EntitySID) goto NextEnt;
						}
					}

					//cut distance
					if (Math::Calc3D_Dist(LocalPlayer.BasePlayer->GetBoneByID(head), Player->GetBoneByID(head)) > Vars::AimBot::Range)
						goto NextEnt;

					//calc visible & low fov
					if (Player->IsVisible() && (FOV > (CurFOV = GetFov(Player, BoneList(Vars::Global::BoneToAim))))) {
						FOV = CurFOV; AimEntity = Player;
					}
				}

				//LP
				else {
					LP_isValid = true;
					LocalPlayer.BasePlayer = Player;
				}
			}

			//camera
			else if (Tag == 5)
				LocalPlayer.pViewMatrix = (Matrix4x4*)(Entity + 0xDC);

			//sky
			else if (Tag == 20011 && Vars::Visuals::AlwaysDay) {
				DWORD64 Dome = safe_read(Entity + 0x28, DWORD64);
				DWORD64 TodCycle = safe_read(Dome + 0x38, DWORD64);
				safe_write(TodCycle + 0x10, 13.37f, float);
			}
		}

		//goto next entity
		NextEnt: continue;
	}

	//GET MEMES
	if (LP_isValid)
	{
		WeaponPatch();
		Aim(AimEntity);
		Misc();
	}

	//LP not valid
	else LocalPlayer.BasePlayer = nullptr;
}