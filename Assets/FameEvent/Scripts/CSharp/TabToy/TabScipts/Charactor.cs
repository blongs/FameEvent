// Generated by github.com/davyxu/tabtoy
// Version: 2.8.9
// DO NOT EDIT!!
using System.Collections.Generic;

namespace TableCharactor
{
	
	

	// Defined in table: Config
	
	public partial class Config
	{
	
		public tabtoy.Logger TableLogger = new tabtoy.Logger();
	
		
		/// <summary> 
		/// Charactor
		/// </summary>
		public List<CharactorDefine> Charactor = new List<CharactorDefine>(); 
	
	
		#region Index code
	 	Dictionary<int, CharactorDefine> _CharactorByID = new Dictionary<int, CharactorDefine>();
        public CharactorDefine GetCharactorByID(int ID, CharactorDefine def = default(CharactorDefine))
        {
            CharactorDefine ret;
            if ( _CharactorByID.TryGetValue( ID, out ret ) )
            {
                return ret;
            }
			
			if ( def == default(CharactorDefine) )
			{
				TableLogger.ErrorLine("GetCharactorByID failed, ID: {0}", ID);
			}

            return def;
        }
		
	
		#endregion
		#region Deserialize code
		
		static tabtoy.DeserializeHandler<Config> _ConfigDeserializeHandler;
		static tabtoy.DeserializeHandler<Config> ConfigDeserializeHandler
		{
			get
			{
				if (_ConfigDeserializeHandler == null )
				{
					_ConfigDeserializeHandler = new tabtoy.DeserializeHandler<Config>(Deserialize);
				}

				return _ConfigDeserializeHandler;
			}
		}
		public static void Deserialize( Config ins, tabtoy.DataReader reader )
		{
			
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0xa0000:
                	{
						ins.Charactor.Add( reader.ReadStruct<CharactorDefine>(CharactorDefineDeserializeHandler) );
                	}
                	break; 
                }
             } 

			
			// Build Charactor Index
			for( int i = 0;i< ins.Charactor.Count;i++)
			{
				var element = ins.Charactor[i];
				
				ins._CharactorByID.Add(element.ID, element);
				
			}
			
		}
		static tabtoy.DeserializeHandler<CharactorDefine> _CharactorDefineDeserializeHandler;
		static tabtoy.DeserializeHandler<CharactorDefine> CharactorDefineDeserializeHandler
		{
			get
			{
				if (_CharactorDefineDeserializeHandler == null )
				{
					_CharactorDefineDeserializeHandler = new tabtoy.DeserializeHandler<CharactorDefine>(Deserialize);
				}

				return _CharactorDefineDeserializeHandler;
			}
		}
		public static void Deserialize( CharactorDefine ins, tabtoy.DataReader reader )
		{
			
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.ID = reader.ReadInt32();
                	}
                	break; 
                	case 0x60001:
                	{
						ins.Name = reader.ReadString();
                	}
                	break; 
                	case 0x10002:
                	{
						ins.IsMain = reader.ReadInt32();
                	}
                	break; 
                	case 0x10003:
                	{
						ins.IsOpen = reader.ReadInt32();
                	}
                	break; 
                	case 0x10004:
                	{
						ins.IsOpenJX = reader.ReadInt32();
                	}
                	break; 
                	case 0x50005:
                	{
						ins.Totle = reader.ReadFloat();
                	}
                	break; 
                	case 0x10006:
                	{
						ins.Attack = reader.ReadInt32();
                	}
                	break; 
                	case 0x10007:
                	{
						ins.AP = reader.ReadInt32();
                	}
                	break; 
                	case 0x10008:
                	{
						ins.HJ = reader.ReadInt32();
                	}
                	break; 
                	case 0x10009:
                	{
						ins.FK = reader.ReadInt32();
                	}
                	break; 
                	case 0x1000a:
                	{
						ins.HP = reader.ReadInt32();
                	}
                	break; 
                	case 0x1000b:
                	{
						ins.Speed = reader.ReadInt32();
                	}
                	break; 
                	case 0x1000c:
                	{
						ins.Scale = reader.ReadInt32();
                	}
                	break; 
                	case 0x1000d:
                	{
						ins.P1 = reader.ReadInt32();
                	}
                	break; 
                	case 0x1000e:
                	{
						ins.P2 = reader.ReadInt32();
                	}
                	break; 
                	case 0x1000f:
                	{
						ins.P3 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10010:
                	{
						ins.P4 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10011:
                	{
						ins.P5 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10012:
                	{
						ins.P6 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10013:
                	{
						ins.H1 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10014:
                	{
						ins.H2 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10015:
                	{
						ins.H3 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10016:
                	{
						ins.H4 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10017:
                	{
						ins.H5 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10018:
                	{
						ins.H6 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10019:
                	{
						ins.NP = reader.ReadInt32();
                	}
                	break; 
                	case 0x1001a:
                	{
						ins.NH = reader.ReadInt32();
                	}
                	break; 
                	case 0x6001b:
                	{
						ins.Resource = reader.ReadString();
                	}
                	break; 
                	case 0x6001c:
                	{
						ins.PZ = reader.ReadString();
                	}
                	break; 
                	case 0x6001d:
                	{
						ins.Per = reader.ReadString();
                	}
                	break; 
                	case 0x1001e:
                	{
						ins.PerSkill = reader.ReadInt32();
                	}
                	break; 
                	case 0x1001f:
                	{
						ins.Skill1 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10020:
                	{
						ins.Skill2 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10021:
                	{
						ins.Skill3 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10022:
                	{
						ins.Skill4 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10023:
                	{
						ins.Skill5 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10024:
                	{
						ins.Skill6 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10025:
                	{
						ins.Skill7 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10026:
                	{
						ins.TalkID = reader.ReadInt32();
                	}
                	break; 
                }
             } 

			
		}
		#endregion
	

	} 

	// Defined in table: Charactor
	[System.Serializable]
	public partial class CharactorDefine
	{
	
		
		/// <summary> 
		/// 唯一ID
		/// </summary>
		public int ID = 0; 
		
		/// <summary> 
		/// 名称
		/// </summary>
		public string Name = ""; 
		
		/// <summary> 
		/// 是否主角
		/// </summary>
		public int IsMain = 0; 
		
		/// <summary> 
		/// 是否开放
		/// </summary>
		public int IsOpen = 0; 
		
		/// <summary> 
		/// 是否开放觉醒
		/// </summary>
		public int IsOpenJX = 0; 
		
		/// <summary> 
		/// 总和
		/// </summary>
		public float Totle = 0f; 
		
		/// <summary> 
		/// 攻击
		/// </summary>
		public int Attack = 0; 
		
		/// <summary> 
		/// 法强
		/// </summary>
		public int AP = 0; 
		
		/// <summary> 
		/// 护甲
		/// </summary>
		public int HJ = 0; 
		
		/// <summary> 
		/// 法抗
		/// </summary>
		public int FK = 0; 
		
		/// <summary> 
		/// 生命
		/// </summary>
		public int HP = 0; 
		
		/// <summary> 
		/// 速度
		/// </summary>
		public int Speed = 0; 
		
		/// <summary> 
		/// 缩放比例
		/// </summary>
		public int Scale = 0; 
		
		/// <summary> 
		/// 形象1
		/// </summary>
		public int P1 = 0; 
		
		/// <summary> 
		/// 形象2
		/// </summary>
		public int P2 = 0; 
		
		/// <summary> 
		/// 形象3
		/// </summary>
		public int P3 = 0; 
		
		/// <summary> 
		/// 形象4
		/// </summary>
		public int P4 = 0; 
		
		/// <summary> 
		/// 形象5
		/// </summary>
		public int P5 = 0; 
		
		/// <summary> 
		/// 形象6
		/// </summary>
		public int P6 = 0; 
		
		/// <summary> 
		/// 头像1
		/// </summary>
		public int H1 = 0; 
		
		/// <summary> 
		/// 头像2
		/// </summary>
		public int H2 = 0; 
		
		/// <summary> 
		/// 头像3
		/// </summary>
		public int H3 = 0; 
		
		/// <summary> 
		/// 头像4
		/// </summary>
		public int H4 = 0; 
		
		/// <summary> 
		/// 头像5
		/// </summary>
		public int H5 = 0; 
		
		/// <summary> 
		/// 头像6
		/// </summary>
		public int H6 = 0; 
		
		/// <summary> 
		/// 新觉醒形象
		/// </summary>
		public int NP = 0; 
		
		/// <summary> 
		/// 新觉醒头像
		/// </summary>
		public int NH = 0; 
		
		/// <summary> 
		/// 资源包
		/// </summary>
		public string Resource = ""; 
		
		/// <summary> 
		/// 品质
		/// </summary>
		public string PZ = ""; 
		
		/// <summary> 
		/// 职业
		/// </summary>
		public string Per = ""; 
		
		/// <summary> 
		/// 初始技能数量
		/// </summary>
		public int PerSkill = 0; 
		
		/// <summary> 
		/// 技能id1
		/// </summary>
		public int Skill1 = 0; 
		
		/// <summary> 
		/// 技能id2
		/// </summary>
		public int Skill2 = 0; 
		
		/// <summary> 
		/// 技能id3
		/// </summary>
		public int Skill3 = 0; 
		
		/// <summary> 
		/// 技能id4
		/// </summary>
		public int Skill4 = 0; 
		
		/// <summary> 
		/// 技能id5
		/// </summary>
		public int Skill5 = 100; 
		
		/// <summary> 
		/// 技能id6
		/// </summary>
		public int Skill6 = 100; 
		
		/// <summary> 
		/// 技能id7
		/// </summary>
		public int Skill7 = 100; 
		
		/// <summary> 
		/// 大厅对话ID
		/// </summary>
		public int TalkID = 0; 
	
	

	} 

}