//game structs
const unsigned short Crc16Table[256] = {
0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7,
0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD, 0xE1CE, 0xF1EF,
0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6,
0x9339, 0x8318, 0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE,
0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485,
0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D,
0x3653, 0x2672, 0x1611, 0x0630, 0x76D7, 0x66F6, 0x5695, 0x46B4,
0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC,
0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823,
0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B,
0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12,
0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A,
0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41,
0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49,
0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70,
0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78,
0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F,
0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067,
0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E,
0x02B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256,
0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D,
0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C,
0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634,
0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB,
0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3,
0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A,
0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1, 0x1AD0, 0x2AB3, 0x3A92,
0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9,
0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1,
0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8,
0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0
};

enum BoneList : int
{
	l_hip = 1,
	l_knee,
	l_foot,
	l_toe,
	l_ankle_scale,
	pelvis,
	penis,
	GenitalCensor,
	GenitalCensor_LOD0,
	Inner_LOD0,
	GenitalCensor_LOD1,
	GenitalCensor_LOD2,
	r_hip,
	r_knee,
	r_foot,
	r_toe,
	r_ankle_scale,
	spine1,
	spine1_scale,
	spine2,
	spine3,
	spine4,
	l_clavicle,
	l_upperarm,
	l_forearm,
	l_hand,
	l_index1,
	l_index2,
	l_index3,
	l_little1,
	l_little2,
	l_little3,
	l_middle1,
	l_middle2,
	l_middle3,
	l_prop,
	l_ring1,
	l_ring2,
	l_ring3,
	l_thumb1,
	l_thumb2,
	l_thumb3,
	IKtarget_righthand_min,
	IKtarget_righthand_max,
	l_ulna,
	neck,
	head,
	jaw,
	eyeTranform,
	l_eye,
	l_Eyelid,
	r_eye,
	r_Eyelid,
	r_clavicle,
	r_upperarm,
	r_forearm,
	r_hand,
	r_index1,
	r_index2,
	r_index3,
	r_little1,
	r_little2,
	r_little3,
	r_middle1,
	r_middle2,
	r_middle3,
	r_prop,
	r_ring1,
	r_ring2,
	r_ring3,
	r_thumb1,
	r_thumb2,
	r_thumb3,
	IKtarget_lefthand_min,
	IKtarget_lefthand_max,
	r_ulna,
	l_breast,
	r_breast,
	BoobCensor,
	BreastCensor_LOD0,
	BreastCensor_LOD1,
	BreastCensor_LOD2,
	collision,
	displacement
};

typedef struct _UncStr
{
	char stub[0x10];
	int len;
	wchar_t str[1];
} *pUncStr;

class WeaponData
{
private:
	unsigned short CRC(unsigned char* pcBlock, unsigned short len)
	{
		unsigned short crc = 0xFFFF;
		while (len--)
			crc = (crc << 8) ^ Crc16Table[(crc >> 8) ^ *pcBlock++];
		return crc;
	}

public:
	wchar_t* GetShortName(int* len)
	{
		DWORD64 Info = safe_read(this + 0x18, DWORD64);
		pUncStr Str = ((pUncStr)safe_read(Info + 0x20, DWORD64));
		int leng = safe_read(Str + 0x10, int);
		if (!leng) return nullptr;
		if (len)*len = leng;
		return Str->str;
	}

	USHORT GetNameHash() {
		int Len;
		UCHAR* ShortName = (UCHAR*)GetShortName(&Len);
		if (ShortName == nullptr) return 0;
		return CRC(ShortName, (Len * 2));
	}

	wchar_t* GetName() {
		DWORD64 Info = safe_read(this + 0x18, DWORD64);
		DWORD64 DispName = safe_read(Info + 0x28, DWORD64); // Translate.Phrase displayName;
		pUncStr Str = ((pUncStr)safe_read(DispName + 0x18, DWORD64));
		if (!Str) return nullptr; return Str->str;
	}

	int GetUID() {
		return safe_read(this + 0x20, int);
	}

	void AntiSpread()
	{
		if (Vars::Misc::AntiSpread)
		{
			DWORD64 Held = safe_read(this + 0x98, DWORD64);//EntityRef heldEntity
			safe_write(Held + 0x304, 0.f, float); //private float stancePenalty;
			safe_write(Held + 0x308, 0.f, float); //private float aimconePenalty;
			safe_write(Held + 0x2D0, 0.f, float); //public float aimCone;
			safe_write(Held + 0x2D4, 0.f, float); //public float hipAimCone;
			safe_write(Held + 0x2D8, 0.f, float); //public float aimconePenaltyPerShot;
			//Работает
		}
	}

	void NoRecoil()
	{
		if (Vars::Misc::NoRecoil)
		{
			DWORD64 Held = safe_read(this + 0x98, DWORD64);
			DWORD64 recoil = safe_read(Held + 0x2C0, DWORD64); //public RecoilProperties recoil;
			safe_write(recoil + 0x18, 0.f, float); //public float recoilYawMin;
			safe_write(recoil + 0x1C, 0.f, float); //public float recoilYawMax;
			safe_write(recoil + 0x20, 0.f, float); //public float recoilPitchMin;
			safe_write(recoil + 0x24, 0.f, float); //public float recoilPitchMax;		
			safe_write(recoil + 0x30, 0.f, float); //public float ADSScale; 
			safe_write(recoil + 0x34, 0.f, float); //public float movementPenalty; 		
			//Работает
		}
	}

	/*void ProjectileFired()
	{
		DWORD64 Held = safe_read(this + 0x58, DWORD64);
		DWORD64 Created = safe_read(Held + 0x260, DWORD64);
		DWORD64 items = safe_read(Held + 0x10, DWORD64);
		int size = safe_read(Created + 0x18, DWORD64);
		if (size > 0){
			for (int j = 0; j < 6; ++j) {
				DWORD64 Slot = safe_read(items + 0x50, DWORD64);
				if (!Slot) continue;
				safe_write(Slot + 0x9C,1s0.f, float);
			}
		}
	}*/

	void FastReload()
	{
		if (Vars::Misc::FastReload)
		{
			DWORD64 Held = safe_read(this + 0x98, DWORD64);
			safe_write(Held + 0x2A8, 1, bool); //private Animator _animator; // 0x2A8
			//РАБОТАТЬ ПОФИКШЕНО
		}
	}

	void SetAutomatic()
	{
		if (Vars::Misc::Automatic)
		{
			DWORD64 Held = safe_read(this + 0x98, DWORD64); // EntityRef heldEntity
			safe_write(Held + 0x270, 1, bool); // public bool automatic
			//РАБОТАЕТ
		}
	}

	void SuperEoka()
	{
		if (Vars::Misc::SuperEoka)
		{
			DWORD64 Held = safe_read(this + 0x98, DWORD64); // EntityRef heldEntity
			safe_write(Held + 0x340, 1.f, float); //public float successFraction;
			//РАБОТАЕТ
		}
	}

	void SuperBow()
	{
		if (Vars::Misc::SuperBow)
		{
			DWORD64 Held = safe_read(this + 0x98, DWORD64); // EntityRef heldEntity
			safe_write(Held + 0x340, 1, bool); // protected bool attackReady;
			safe_write(Held + 0x344, 1.f, float); // private float arrowBack;
			//РАБОТАЕТ
		}
	}

	void BuildAsUWant()
	{
		DWORD64 Held = safe_read(this + 0x98, DWORD64); // EntityRef heldEntity
		DWORD64 Construction = safe_read(Held + 0x1C8, DWORD64); //public GameObject hornOrigin;
		if (Vars::Misc::BuildAsUWant) safe_write(Construction + 0xC4, 15.f, float);
		if (!Vars::Misc::BuildAsUWant) safe_write(Construction + 0xC4, 180.f, float);
		// было два 0xC4  m_PrevContentBounds  нужен тест!!
	}

	void LongHand()
	{
		DWORD64 Held = safe_read(this + 0x98, DWORD64); // EntityRef heldEntity
		safe_write(Held + 0x270, 4.5f, float); //private Transform attachmentBoneCache; // 0x270
		// РАБОТАЕТ
	}

	void FatHand()
	{
		DWORD64 Held = safe_read(this + 0x98, DWORD64); // EntityRef heldEntity
		safe_write(Held + 0x27C, 15.f, float); //public float attackRadius;
		//РАБОТАЕТ
	}

	void RunOnHit()
	{
		DWORD64 Held = safe_read(this + 0x98, DWORD64); // EntityRef heldEntity
		safe_write(Held + 0x281, 0, int); //public bool blockSprintOnAttack;
		//РАБОТАЕТ
	}

	//Flex feature
	void SkinChanger(DWORD64 skinid)
	{
		if (Vars::Misc::SkinChanger)
		{
			DWORD64 Held = safe_read(this + 0x98, DWORD64);
			safe_write(this + 0x58, skinid, DWORD64);
			safe_write(Held + 0xB8, skinid, DWORD64);
		}
	}
};

class BasePlayer
{
public:
	//*** base entity info ***//
	float GetHealth() {
		return safe_read(this + 0x1F4, float); //private float _health;
		//обновил
	}

	const wchar_t* GetName() {
		pUncStr Str = ((pUncStr)(safe_read(this + 0x618, DWORD64))); //	protected string _displayName
		if (!Str) return L""; return Str->str;
		//обновил
	}

	Vector3 GetVelocity() {
		DWORD64 PlayerModel = safe_read(this + 0x490, DWORD64); //	public PlayerModel playerModel;
		return safe_read(PlayerModel + 0x1EC, Vector3); // 	private Vector3 newVelocity;
		//обновил
	}

	bool IsVisible() {
		if (Vars::AimBot::VisibleCheck) {
			DWORD64 PlayerModel = safe_read(this + 0x490, DWORD64); //public PlayerModel playerModel;
			return safe_read(PlayerModel + 0x238, bool); // internal bool visible;
		}
		else return true;
		//обновил
	}

	float GetTickTime() {
		return safe_read(this + 0x584, float); //	private float lastSentTickTime;
		//обновил
	}

	bool IsValid(bool LPlayer = false) {
		return (((LPlayer || Vars::Visuals::IgnoreSleepers) ? !HasFlags(16) : true) && !IsDead() && (GetHealth() >= 0.8f));
	}

	bool HasFlags(int Flg) {
		return (safe_read(this + 0x5B0, int) & Flg); //	public BasePlayer.PlayerFlags playerFlags;
		//обновил
	}

	Vector3 GetBoneByID(BoneList BoneID) {
		return GetPosition(GetTransform(BoneID));
	}

	int GetTeamSize()
	{
		DWORD64 ClientTeam = safe_read(this + 0x500, DWORD64); // public PlayerTeam clientTeam
		DWORD64 members = safe_read(ClientTeam + 0x28, DWORD64);//	private ListHashSet`1<ILOD> members; // 0x28 ���  public List`1<PlayerTeam.TeamMember> members; // 0x28
		return safe_read(members + 0x18, DWORD64);
	}

	DWORD64 IsTeamMate(int Id)
	{
		DWORD64 ClientTeam = safe_read(this + 0x500, DWORD64);//	public PlayerTeam clientTeam;
		DWORD64 members = safe_read(ClientTeam + 0x28, DWORD64);//	private ListHashSet`1<ILOD> members; // 0x28 ���  public List`1<PlayerTeam.TeamMember> members; // 0x28
		DWORD64 List = safe_read(members + 0x10, DWORD64);
		DWORD64 player = safe_read(List + 0x20 + (Id * 0x8), DWORD64); // 	private BasePlayer player;
		return safe_read(player + 0x18, DWORD64);
	}

	bool IsDead() {
		if (!this) return true;
		return safe_read(this + 0x1EC, bool); //	public BaseCombatEntity.LifeState lifestate;
	}

	DWORD64 GetSteamID() {
		return safe_read(this + 0x608, DWORD64);//	public ulong userID;
	}

	//*** localplayer ***//
	bool IsMenu() {
		if (!this) return true;
		DWORD64 Input = safe_read(this + 0x5D8, DWORD64); //	public PlayerInput input;
		return !(safe_read(Input + 0x94, bool));// private bool hasKeyFocus;
	}

	void SetVA(const Vector2& VA) {
		DWORD64 Input = safe_read(this + 0x5D8, DWORD64);//	public PlayerInput input;
		safe_write(Input + 0x3C, VA, Vector2); //private Vector3 bodyAngles;
	}

	void SetRA() {
		DWORD64 Input = safe_read(this + 0x5D8, DWORD64);//	public PlayerInput input;
		Vector2 RA = { 0.f, 0.f };
		safe_write(Input + 0x64, RA, Vector2); //public Vector3 recoilAngles;
	}

	Vector2 GetVA() {
		DWORD64 Input = safe_read(this + 0x5D8, DWORD64);//	public PlayerInput input;
		return safe_read(Input + 0x3C, Vector2);//private Vector3 bodyAngles;
	}

	Vector2 GetRA() {
		DWORD64 Input = safe_read(this + 0x5D8, DWORD64);//	public PlayerInput input;
		return safe_read(Input + 0x64, Vector2);//public Vector3 recoilAngles;
	}

	WeaponData* GetWeaponInfo(int Id) {
		DWORD64 Inventory = safe_read(this + 0x5C0, DWORD64);//	public PlayerInventory inventory;
		DWORD64 Belt = safe_read(Inventory + 0x28, DWORD64);
		DWORD64 ItemList = safe_read(Belt + 0x38, DWORD64);// 	public List`1<Item> itemList;
		DWORD64 Items = safe_read(ItemList + 0x10, DWORD64); //	public List`1<InventoryItem.Amount> items;
		return (WeaponData*)safe_read(Items + 0x20 + (Id * 0x8), DWORD64);
	}

	WeaponData* GetActiveWeapon() {
		int ActUID = safe_read(this + 0x530, int); //	private uint clActiveItem;
		if (!ActUID) return nullptr;
		WeaponData* ActiveWeapon;
		for (int i = 0; i < 6; i++)
			if (ActUID == (ActiveWeapon = GetWeaponInfo(i))->GetUID())
				return ActiveWeapon;
		return nullptr;
	}

	//*** cheat func ***//
	void AirStuck(bool state) {
		safe_write(this + 0x498, state, bool); //public bool Frozen; // 0x528??
		//РАБОТАЕТ
	}

	void FakeAdmin(int Val) {

		int Flags = safe_read(this + 0x5B0, int); //public BasePlayer.PlayerFlags playerFlags;
		safe_write(this + 0x5B0, (Flags |= 4), int);

	}

	void SpiderMan() {
		DWORD64 Movement = safe_read(this + 0x5E0, DWORD64); //public BaseMovement movement;
		safe_write(Movement + 0xAC, 0.f, float);//private float groundAngle;
		safe_write(Movement + 0xB0, 0.f, float);//private float groundAngleNew;
		//РАБОТАЕТ
	}

	void WaterBoost() {
		safe_write(this + 0x63C, 0.15f, float); //	public float clothingWaterSpeedBonus;
		//РАБОТАЕТ
	}

	void NoSway() {
		safe_write(this + 0x640, 1.f, float);//	public float clothingAccuracyBonus;
		//РАБОТАЕТ
	}



	void NoBlockAiming() {
		safe_write(this + 0x634, false, bool);//public bool clothingBlocksAiming;
		//РАБОТАЕТ
	}

	void NoHeavyReduct() {
		float Reduct = safe_read(this + 0x638, float);//	public float clothingMoveSpeedReduction;
		if (Reduct == 0.f) { safe_write(this + 0x638, -0.1f, float); }//	public float clothingMoveSpeedReduction;
		else if ((Reduct > 0.15f) && (Reduct < 0.35f)) {
			safe_write(this + 0x638, 0.15f, float);//	public float clothingMoveSpeedReduction;
		}
		else if (Reduct > 0.39f) {
			safe_write(this + 0x638, 0.15f, float);//	public float clothingMoveSpeedReduction;
		}
		//РАБОТАЕТ
	}

	void SetGravity(float val) {
		DWORD64 Movement = safe_read(this + 0x5E0, DWORD64);// BaseMovement movement
		safe_write(Movement + 0x74, val, float); //public float gravityMultiplier;
		//РАБОТАЕТ
	}

private:
	Vector3 GetPosition(DWORD64 transform)
	{
		if (!transform) return Vector3{ 0.f, 0.f, 0.f };

		struct Matrix34 { BYTE vec0[16]; BYTE vec1[16]; BYTE vec2[16]; };
		const __m128 mulVec0 = { -2.000, 2.000, -2.000, 0.000 };
		const __m128 mulVec1 = { 2.000, -2.000, -2.000, 0.000 };
		const __m128 mulVec2 = { -2.000, -2.000, 2.000, 0.000 };

		int Index = *(PINT)(transform + 0x40);
		DWORD64 pTransformData = safe_read(transform + 0x38, DWORD64);
		DWORD64 transformData[2];
		safe_memcpy(&transformData, (PVOID)(pTransformData + 0x18), 16);

		size_t sizeMatriciesBuf = 48 * Index + 48;
		size_t sizeIndicesBuf = 4 * Index + 4;

		int pIndicesBuf[100];
		Matrix34 pMatriciesBuf[1000];

		safe_memcpy(pMatriciesBuf, (PVOID)transformData[0], sizeMatriciesBuf);
		safe_memcpy(pIndicesBuf, (PVOID)transformData[1], sizeIndicesBuf);

		__m128 result = *(__m128*)((ULONGLONG)pMatriciesBuf + 0x30 * Index);
		int transformIndex = *(int*)((ULONGLONG)pIndicesBuf + 0x4 * Index);

		while (transformIndex >= 0)
		{
			Matrix34 matrix34 = *(Matrix34*)((ULONGLONG)pMatriciesBuf + 0x30 * transformIndex);
			__m128 xxxx = _mm_castsi128_ps(_mm_shuffle_epi32(*(__m128i*)(&matrix34.vec1), 0x00));
			__m128 yyyy = _mm_castsi128_ps(_mm_shuffle_epi32(*(__m128i*)(&matrix34.vec1), 0x55));
			__m128 zwxy = _mm_castsi128_ps(_mm_shuffle_epi32(*(__m128i*)(&matrix34.vec1), 0x8E));
			__m128 wzyw = _mm_castsi128_ps(_mm_shuffle_epi32(*(__m128i*)(&matrix34.vec1), 0xDB));
			__m128 zzzz = _mm_castsi128_ps(_mm_shuffle_epi32(*(__m128i*)(&matrix34.vec1), 0xAA));
			__m128 yxwy = _mm_castsi128_ps(_mm_shuffle_epi32(*(__m128i*)(&matrix34.vec1), 0x71));
			__m128 tmp7 = _mm_mul_ps(*(__m128*)(&matrix34.vec2), result);

			result = _mm_add_ps(
				_mm_add_ps(
					_mm_add_ps(
						_mm_mul_ps(
							_mm_sub_ps(
								_mm_mul_ps(_mm_mul_ps(xxxx, mulVec1), zwxy),
								_mm_mul_ps(_mm_mul_ps(yyyy, mulVec2), wzyw)),
							_mm_castsi128_ps(_mm_shuffle_epi32(_mm_castps_si128(tmp7), 0xAA))),
						_mm_mul_ps(
							_mm_sub_ps(
								_mm_mul_ps(_mm_mul_ps(zzzz, mulVec2), wzyw),
								_mm_mul_ps(_mm_mul_ps(xxxx, mulVec0), yxwy)),
							_mm_castsi128_ps(_mm_shuffle_epi32(_mm_castps_si128(tmp7), 0x55)))),
					_mm_add_ps(
						_mm_mul_ps(
							_mm_sub_ps(
								_mm_mul_ps(_mm_mul_ps(yyyy, mulVec0), yxwy),
								_mm_mul_ps(_mm_mul_ps(zzzz, mulVec1), zwxy)),
							_mm_castsi128_ps(_mm_shuffle_epi32(_mm_castps_si128(tmp7), 0x00))),
						tmp7)), *(__m128*)(&matrix34.vec0));

			transformIndex = *(int*)((ULONGLONG)pIndicesBuf + 0x4 * transformIndex);
		}

		return Vector3(result.m128_f32[0], result.m128_f32[1], result.m128_f32[2]);
	}

	DWORD64 GetTransform(int bone)
	{
		DWORD64 player_model = safe_read(this + 0x118, DWORD64);// public Model model;_public class BaseEntity : BaseNetworkable, IProvider, ILerpTarget, IPrefabPreProcess // TypeDefIndex: 8714
		DWORD64 boneTransforms = safe_read(player_model + 0x48, DWORD64);//public Transform[] boneTransforms;
		DWORD64 BoneValue = safe_read((boneTransforms + (0x20 + (bone * 0x8))), DWORD64);
		return safe_read(BoneValue + 0x10, DWORD64);
	}
	//    DWORD64 GetTransform(int bone)
	   //{
	   //	DWORD64 player_model = safe_read(this + 0x520, DWORD64); // public PlayerModel playerModel;
	   //	DWORD64 multi_mesh = safe_read(player_model + 0x230, DWORD64); //private SkinnedMultiMesh _multiMesh;
	   //	DWORD64 bone_dictionary = safe_read(multi_mesh + 0x28, DWORD64);
	   //	DWORD64 bone_values = safe_read(bone_dictionary + 0x18, DWORD64);
	   //	DWORD64 entity_bone = safe_read((bone_values + (0x20 + ((bone - 1) * 0x8))), DWORD64);
	   //	return safe_read(entity_bone + 0x10, DWORD64);
	   //}
};

//Base Player
class LPlayerBase
{
public:
	BasePlayer* BasePlayer = nullptr;
	Matrix4x4* pViewMatrix = nullptr;
	bool WorldToScreen(const Vector3& EntityPos, Vector2& ScreenPos)
	{
		if (!pViewMatrix) return false;
		Vector3 TransVec = Vector3(pViewMatrix->_14, pViewMatrix->_24, pViewMatrix->_34);
		Vector3 RightVec = Vector3(pViewMatrix->_11, pViewMatrix->_21, pViewMatrix->_31);
		Vector3 UpVec = Vector3(pViewMatrix->_12, pViewMatrix->_22, pViewMatrix->_32);
		float w = Math::Dot(TransVec, EntityPos) + pViewMatrix->_44;
		if (w < 0.098f) return false;
		float y = Math::Dot(UpVec, EntityPos) + pViewMatrix->_42;
		float x = Math::Dot(RightVec, EntityPos) + pViewMatrix->_41;
		ScreenPos = Vector2((Vars::Global::ScreenWidth / 2) * (1.f + x / w), (Vars::Global::ScreenHigh / 2) * (1.f - y / w));
		return true;
	}

};

DECLSPEC_NOINLINE void Flex() {
	FC(kernel32, Sleep, 0);
}

LPlayerBase LocalPlayer;