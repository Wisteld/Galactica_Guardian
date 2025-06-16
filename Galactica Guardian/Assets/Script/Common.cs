using System.Runtime.InteropServices;
using UnityEngine.Experimental.Rendering;

namespace Common
{
    class Com
    {
        #region デバッグモード.
        /// <summary>
        /// システム系デバッグモード.
        /// </summary>
        public const bool DEBUG_MODE_SYSTEM = true;

        /// <summary>
        /// エネミー系デバッグモード.
        /// </summary>
        public const bool DEBUG_MODE_ENEMY = true;

        /// <summary>
        /// プレイヤー系デバッグモード.
        /// </summary>
        public const bool DEBUG_MODE_PLAYER = true;
        #endregion

        #region プレイヤー.
        /// <summary>
        /// プレイヤーの初期速度.
        /// </summary>
        public const float PLAYER_SPEED = 3.0f;

        /// <summary>
        /// プレイヤーの速度上昇倍率.
        /// </summary>
        public const float PLAYER_SPEED_RATE = 0.5f;

        /// <summary>
        /// プレイヤーの初期体力.
        /// </summary>
        public const int PLAYER_HP = 3;

        /// <summary>
        /// プレイヤーの被弾時の点滅間隔.
        /// </summary>
        public const float PLAYER_BLINK = 7f;

        /// <summary>
        /// プレイヤーの被弾時の無敵時間.
        /// </summary>
        public const float PLAYER_INVINCIBLE_TIME = 1.5f;

        /// <summary>
        /// プレイヤー攻撃の弾速.
        /// </summary>
        public const float PLAYER_BULLET_SPEED = 15.0f;

        /// <summary>
        /// プレイヤーの弾が消えるまでの時間.
        /// </summary>
        public const float PLAYER_BULLET_DELETE_TIME = 0.7f;

        /// <summary>
        /// プレイヤーのミサイルの弾速.
        /// </summary>
        public const float PLAYER_MISSILE_SPEED = 7.0f;

        /// <summary>
        /// プレイヤーのミサイルの旋回速度.
        /// </summary>
        public const float PLAYER_MISSILE_ROTATE_SPEED = 60f;

        /// <summary>
        /// プレイヤーのミサイルが消えるまでの時間.
        /// </summary>
        public const float PLAYER_MISSILE_DELETE_TIME = 1.5f;

        /// <summary>
        /// ミサイルの発射レート.
        /// </summary>
        public const float PLAYER_MISSILE_RATE = 0.8f;

        /// <summary>
        /// プレイヤーのバリアの耐久値.
        /// </summary>
        public const int PLAYER_BARRIER_HP = 12;

        #region パワーアップナンバー.

        /// <summary>
        /// パワーアップ01：レーザー.
        /// </summary>
        public const int PLAYER_POWER_UP_LASER = 1;

        /// <summary>
        /// パワーアップ02：ミサイル.
        /// </summary>
        public const int PLAYER_POWER_UP_MISSILE = 2;

        /// <summary>
        /// パワーアップ03：弾二発同時発射.
        /// </summary>
        public const int PLAYER_POWER_UP_TWINSHOT = 3;

        /// <summary>
        /// パワーアップ04：ミサイル二発同時発射.
        /// </summary>
        public const int PLAYER_POWER_UP_TWINMISSILE = 4;

        /// <summary>
        /// パワーアップ05：バリア展開.
        /// </summary>
        public const int PLAYER_POWER_UP_BARRIER = 5;
        #endregion

        #endregion

        #region エネミー.
        /// <summary>
        /// エネミーの移動速度.
        /// </summary>
        public const float ENEMY_SPEED = 1.8f;

        /// <summary>
        /// エネミーの横移動速度.
        /// </summary>
        public const float ENEMY_SIDE_SPEED = 1.5f;

        /// <summary>
        /// エネミーの体力.
        /// </summary>
        public const int ENEMY_HP = 2;

        /// <summary>
        /// エネミーの壁との距離倍率.
        /// </summary>
        public const float ENEMY_DISTANCE = 1.5f;

        /// <summary>
        /// 横方向への移動抽選間隔.
        /// </summary>
        public const float ENEMY_SIDE_TIME = 1.0f;

        /// <summary>
        /// エネミー攻撃の弾速.
        /// </summary>
        public const float ENEMY_BULLET_SPEED = 10.0f;

        /// <summary>
        /// エネミーの弾が消えるまでの時間.
        /// </summary>
        public const float ENEMY_BULLET_DELETE = 1.5f;

        /// <summary>
        /// エネミーの発射間隔下限.
        /// </summary>
        public const float ENEMY_FIRE_RND_MIN = 0.6f;

        /// <summary>
        /// エネミーの発射間隔上限.
        /// </summary>
        public const float ENEMY_FIRE_RND_MAX = 1.2f;

        /// <summary>
        /// 自機狙い弾の索敵外範囲.
        /// </summary>
        public const float ENEMY_IGNORE_RANGE = 100f;

        /// <summary>
        /// エネミーのミサイル弾速.
        /// </summary>
        public const float ENEMY_MISSILE_SPEED = 5f;

        /// <summary>
        /// エネミーのミサイル旋回速度.
        /// </summary>
        public const float ENEMY_MISSILE_ROTATE_SPEED = 20f;

        /// <summary>
        /// エネミーのミサイル消滅までの時間.
        /// </summary>
        public const float ENEMY_MISSILE_DELETE_TIME = 2f;

        /// <summary>
        /// 通常弾被弾ダメージ.
        /// </summary>
        public const int ENEMY_DAMAGE_BULLET = 1;

        /// <summary>
        /// ミサイル被弾ダメージ.
        /// </summary>
        public const int ENEMY_DAMAGE_MISSILE = 2;
        #endregion

        #region エネミーα
        /// <summary>
        /// エネミーαの移動速度.
        /// </summary>
        public const float ENEMY_SPEED_α = 3.4f;

        /// <summary>
        /// エネミーαの体力.
        /// </summary>
        public const int ENEMY_HP_α = 4;
        #endregion

        #region エネミーβ
        /// <summary>
        /// エネミーβの移動速度.
        /// </summary>
        public const float ENEMY_SPEED_β = 2.6f;

        /// <summary>
        /// エネミーβの攻撃間隔.
        /// </summary>
        public const float ENEMY_FIRE_RATE_β = 3.0f;



        /// <summary>
        /// エネミーβの体力.
        /// </summary>
        public const int ENEMY_HP_β = 6;
        #endregion

        #region その他.
        /// <summary>
        /// アイテムキャリアーの移動速度.
        /// </summary>
        public const float CARRIER_SPEED = 1.5f;

        /// <summary>
        /// パワーアップ：ウェポンの移動速度.
        /// </summary>
        public const float POWERUP_WEAPON_SPEED = 1.3f;

        /// <summary>
        /// パワーアップ：スピードの移動速度.
        /// </summary>
        public const float POWERUP_SPEED_SPEED = 1.6f;
        #endregion
    }

    class tags
    {
        /// <summary>
        /// エネミータグ.
        /// </summary>
        public const string ENEMY = "Enemy";

        /// <summary>
        /// プレイヤータグ.
        /// </summary>
        public const string PLAYER = "Player";

        /// <summary>
        /// エネミーバレットタグ.
        /// </summary>
        public const string ENEMY_BULLET = "Enemy_Bullet";

        /// <summary>
        /// パワーアップウェポンタグ.
        /// </summary>
        public const string POWERUP_WEAPON = "PowerUp_Weapon";

        /// <summary>
        /// パワーアップスピードタグ.
        /// </summary>
        public const string POWERUP_SPEED = "PowerUp_Speed";

        /// <summary>
        /// プレイヤーバレットタグ.
        /// </summary>
        public const string PLAYER_BULLET = "Player_Bullet";

        /// <summary>
        /// プレイヤーバレットレーザータグ.
        /// </summary>
        public const string PLAYER_BULLET_LASER = "Player_Bullet_Laser";

        /// <summary>
        /// プレイヤーミサイルタグ.
        /// </summary>
        public const string PLAYER_MISSILE = "Player_Missile";
    }
}
