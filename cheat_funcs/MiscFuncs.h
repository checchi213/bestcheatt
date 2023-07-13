void Misc()
{
	if (Vars::Misc::HighJump) {
		if (FC(user32, GetAsyncKeyState, VK_MBUTTON))
			LocalPlayer.BasePlayer->SetGravity(Vars::Misc::JumpValue);
		else LocalPlayer.BasePlayer->SetGravity(3.0f);
	}

	//air stuck
	if (Vars::Misc::AirStuck) {
		if (FC(user32, GetAsyncKeyState, VK_XBUTTON1))
			LocalPlayer.BasePlayer->AirStuck(true);
		else LocalPlayer.BasePlayer->AirStuck(false);
	}

	//boost speed on water
	if (Vars::Misc::WaterBoost)
		LocalPlayer.BasePlayer->WaterBoost();

	//on aim on kastrulya
	if (Vars::Misc::AntiKastrulya)
		LocalPlayer.BasePlayer->NoBlockAiming();

	//boost speed on heavy armor
	if (Vars::Misc::NoHeavyReduct)
		LocalPlayer.BasePlayer->NoHeavyReduct();

	//spider man
	if (Vars::Misc::SpiderMan)
		LocalPlayer.BasePlayer->SpiderMan();

	//remove weapon sway
	if (Vars::Misc::NoSway)
		LocalPlayer.BasePlayer->NoSway();
	if (Vars::Misc::FakeAdmin)
		LocalPlayer.BasePlayer->FakeAdmin(4);

}
