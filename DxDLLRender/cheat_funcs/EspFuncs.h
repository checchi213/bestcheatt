void Skeleton(BasePlayer* BasePlayer)
{
	BoneList Bones[15] = {
		/*LF*/l_foot, l_knee, l_hip,
		/*RF*/r_foot, r_knee, r_hip,
		/*BD*/spine1, neck, head,
		/*LH*/l_upperarm, l_forearm, l_hand,
		/*RH*/r_upperarm, r_forearm, r_hand
	}; Vector2 BonesPos[15];

	//get bones screen pos
	for (int j = 0; j < 15; j++) {
		if (!LocalPlayer.WorldToScreen(BasePlayer->GetBoneByID(Bones[j]), BonesPos[j]))
			return;
	}

	//draw main lines
	for (int j = 0; j < 15; j += 3) {
		GUI::Render.Line(Vector2{ BonesPos[j].x, BonesPos[j].y }, Vector2{ BonesPos[j + 1].x, BonesPos[j + 1].y }, D2D1::ColorF::Gold, 3.f);
		GUI::Render.Line(Vector2{ BonesPos[j].x, BonesPos[j].y }, Vector2{ BonesPos[j + 1].x, BonesPos[j + 1].y }, D2D1::ColorF::Magenta);
		GUI::Render.Line(Vector2{ BonesPos[j + 1].x, BonesPos[j + 1].y }, Vector2{ BonesPos[j + 2].x, BonesPos[j + 2].y }, D2D1::ColorF::Gold, 3.f);
		GUI::Render.Line(Vector2{ BonesPos[j + 1].x, BonesPos[j + 1].y }, Vector2{ BonesPos[j + 2].x, BonesPos[j + 2].y }, D2D1::ColorF::Magenta);
	}

	//draw add lines
	GUI::Render.Line(Vector2{ BonesPos[2].x, BonesPos[2].y }, Vector2{ BonesPos[6].x, BonesPos[6].y }, D2D1::ColorF::Gold, 3.f);
	GUI::Render.Line(Vector2{ BonesPos[2].x, BonesPos[2].y }, Vector2{ BonesPos[6].x, BonesPos[6].y }, D2D1::ColorF::Magenta);
	GUI::Render.Line(Vector2{ BonesPos[5].x, BonesPos[5].y }, Vector2{ BonesPos[6].x, BonesPos[6].y }, D2D1::ColorF::Gold, 3.f);
	GUI::Render.Line(Vector2{ BonesPos[5].x, BonesPos[5].y }, Vector2{ BonesPos[6].x, BonesPos[6].y }, D2D1::ColorF::Magenta);
	GUI::Render.Line(Vector2{ BonesPos[9].x, BonesPos[9].y }, Vector2{ BonesPos[7].x, BonesPos[7].y }, D2D1::ColorF::Gold, 3.f);
	GUI::Render.Line(Vector2{ BonesPos[9].x, BonesPos[9].y }, Vector2{ BonesPos[7].x, BonesPos[7].y }, D2D1::ColorF::Magenta);
	GUI::Render.Line(Vector2{ BonesPos[12].x, BonesPos[12].y }, Vector2{ BonesPos[7].x, BonesPos[7].y }, D2D1::ColorF::Gold, 3.f);
	GUI::Render.Line(Vector2{ BonesPos[12].x, BonesPos[12].y }, Vector2{ BonesPos[7].x, BonesPos[7].y }, D2D1::ColorF::Magenta);
}

void MemeBox(const D2D1::ColorF& PlayerClr, BasePlayer* BasePlayer)
{
	BoneList Bones[4] = {
		/*UP*/l_upperarm, r_upperarm,
		/*DOWN*/r_foot, l_foot
	}; Vector2 BonesPos[4];

	//get bones screen pos
	for (int j = 0; j < 4; j++) {
		if (!LocalPlayer.WorldToScreen(BasePlayer->GetBoneByID(Bones[j]), BonesPos[j]))
			return;
	}

	//draw main lines
	GUI::Render.Line(Vector2{ BonesPos[0].x, BonesPos[0].y }, Vector2{ BonesPos[1].x, BonesPos[1].y }, D2D1::ColorF::Purple, 3.f);
	GUI::Render.Line(Vector2{ BonesPos[0].x, BonesPos[0].y }, Vector2{ BonesPos[1].x, BonesPos[1].y }, PlayerClr);
	GUI::Render.Line(Vector2{ BonesPos[0].x, BonesPos[0].y }, Vector2{ BonesPos[3].x, BonesPos[3].y }, D2D1::ColorF::Purple, 3.f);
	GUI::Render.Line(Vector2{ BonesPos[0].x, BonesPos[0].y }, Vector2{ BonesPos[3].x, BonesPos[3].y }, PlayerClr);
	GUI::Render.Line(Vector2{ BonesPos[2].x, BonesPos[2].y }, Vector2{ BonesPos[1].x, BonesPos[1].y }, D2D1::ColorF::Purple, 3.f);
	GUI::Render.Line(Vector2{ BonesPos[2].x, BonesPos[2].y }, Vector2{ BonesPos[1].x, BonesPos[1].y }, PlayerClr);
	GUI::Render.Line(Vector2{ BonesPos[2].x, BonesPos[2].y }, Vector2{ BonesPos[3].x, BonesPos[3].y }, D2D1::ColorF::Purple, 3.f);
	GUI::Render.Line(Vector2{ BonesPos[2].x, BonesPos[2].y }, Vector2{ BonesPos[3].x, BonesPos[3].y }, PlayerClr);
}

void Crosshair()
{
	float xs = Vars::Global::ScreenWidth / 2, ys = Vars::Global::ScreenHigh / 2;
	GUI::Render.Line(Vector2{ xs, ys - 6 }, Vector2{ xs , ys + 6 }, D2D1::ColorF::Black, 3.5f);
	GUI::Render.Line(Vector2{ xs - 6, ys }, Vector2{ xs + 6, ys }, D2D1::ColorF::Black, 3.5f);
	GUI::Render.Line(Vector2{ xs, ys - 5 }, Vector2{ xs , ys + 5 }, D2D1::ColorF::Purple);
	GUI::Render.Line(Vector2{ xs - 5, ys }, Vector2{ xs + 5, ys }, D2D1::ColorF::Purple);
}

void DrawFOV()
{
	float xs = Vars::Global::ScreenWidth / 2, ys = Vars::Global::ScreenHigh / 2;
	GUI::Render.Ñircle(Vector2{ xs, ys }, D2D1::ColorF::Black, Vars::AimBot::Fov, 3.f);
	GUI::Render.Ñircle(Vector2{ xs, ys }, D2D1::ColorF::Purple, Vars::AimBot::Fov);
}



#pragma region Player ESP

void ESP(BasePlayer* BP, BasePlayer* LP)
{
	bool PlayerSleeping = BP->HasFlags(16);
	if (Vars::Visuals::IgnoreSleepers && PlayerSleeping)
		return;

	if (Vars::Visuals::SkeletonPlayer && BP->GetSteamID() > 1000000000)
	{
		Skeleton(BP);
	}
	else if (Vars::Visuals::SkeletonBots && BP->GetSteamID() < 1000000000)
	{
		Skeleton(BP);
	}

	Vector2 tempFeetR, tempFeetL;
	if (LocalPlayer.WorldToScreen(BP->GetBoneByID(r_foot), tempFeetR) && LocalPlayer.WorldToScreen(BP->GetBoneByID(l_foot), tempFeetL))
	{
		Vector2 tempHead;
		if (LocalPlayer.WorldToScreen(BP->GetBoneByID(jaw) + Vector3(0.f, 0.16f, 0.f), tempHead))
		{
			Vector2 tempFeet = (tempFeetR + tempFeetL) / 2.f;
			float Entity_h = tempHead.y - tempFeet.y;
			float w = Entity_h / 4;
			float Entity_x = tempFeet.x - w;
			float Entity_y = tempFeet.y;
			float Entity_w = Entity_h / 2;

			bool PlayerWounded = BP->HasFlags(64);
			D2D1::ColorF::Enum PlayerClr = PlayerSleeping ? D2D1::ColorF::BlueViolet : PlayerWounded ? D2D1::ColorF::DarkOrange : D2D1::ColorF::Gold;

			int CurPos = 0;

			if (Vars::Visuals::PlayerESP && BP->GetSteamID() > 1000000000)
			{
				GUI::Render.String(Vector2{ (Entity_x + 7), (Entity_y + Entity_h) }, BP->GetName(), PlayerClr);
				CurPos += 15;


				if (Vars::Visuals::ShowPlayerBox)
				{
					if (!PlayerWounded && !PlayerSleeping)
					{
						GUI::Render.Rectangle(Vector2{ Entity_x, Entity_y }, Vector2{ Entity_w, Entity_h }, { 0.f, 0.f, 0.f, 255.f }, 3.f);
						GUI::Render.Rectangle(Vector2{ Entity_x, Entity_y }, Vector2{ Entity_w, Entity_h }, PlayerClr);
					}
					else MemeBox(PlayerClr, BP);

					if (Vars::Visuals::ShowPlayerHealth) {

						GUI::Render.String(Vector2{ (Entity_x + 7), (Entity_y + Entity_h + CurPos) }, FC_FORMAT_W(L"%d HP", (int)BP->GetHealth()), PlayerClr);
						CurPos += 15;
					}

					if (Vars::Visuals::ShowPlayerWeapon)
					{
						const wchar_t* ActiveWeaponName;
						WeaponData* ActWeapon = BP->GetActiveWeapon();
						ActiveWeaponName = ActWeapon->GetName();
						if (!ActWeapon)
						{
							ActiveWeaponName = L"-";

						}
						else
						{
							ActiveWeaponName = ActWeapon->GetName();
						}

						GUI::Render.String(Vector2{ (Entity_x + 7), (Entity_y + Entity_h + CurPos) }, ActiveWeaponName, PlayerClr);
						CurPos += 15;
					}

					if (Vars::Visuals::ShowPlayerDist) {
						GUI::Render.String(Vector2{ (Entity_x + 7), (Entity_y + Entity_h + CurPos) }, FC_FORMAT_W(L"%d : M", (int)Math::Calc3D_Dist(LP->GetBoneByID(head), BP->GetBoneByID(head))), PlayerClr);
						CurPos += 15;
					}
				}
			}
			else if (Vars::Visuals::BotsESP && BP->GetSteamID() < 1000000000)
			{
				D2D1::ColorF bots = D2D1::ColorF::Snow;
				GUI::Render.String(Vector2{ (Entity_x + 7), (Entity_y + Entity_h) }, L"Bots", bots);
				CurPos += 15;
				GUI::Render.String(Vector2{ (Entity_x + 7), (Entity_y + Entity_h + CurPos) }, FC_FORMAT_W(L"%d : HP", (int)BP->GetHealth()), bots);
				CurPos += 15;
				GUI::Render.String(Vector2{ (Entity_x + 7), (Entity_y + Entity_h + CurPos) }, FC_FORMAT_W(L"%d : M", (int)Math::Calc3D_Dist(LP->GetBoneByID(head), BP->GetBoneByID(head))), bots);
				CurPos += 15;

			}
		}
	}
}

#pragma endregion


