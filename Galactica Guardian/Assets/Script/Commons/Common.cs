namespace Common
{
    static class Com
    {
        #region デバッグモード.
        /// <summary>
        /// システム系デバッグモード.
        /// </summary>
        public const bool DEBUG_MODE_SYSTEM = false;

        /// <summary>
        /// エネミー系デバッグモード.
        /// </summary>
        public const bool DEBUG_MODE_ENEMY = false;

        /// <summary>
        /// プレイヤー系デバッグモード.
        /// </summary>
        public const bool DEBUG_MODE_PLAYER = false;
        #endregion

        #region システム.
        /// <summary>
        /// SE再生用オーディオソースのリスト数.
        /// </summary>
        public const int SE_LIST_MAX = 40;
        #endregion

        #region プレイヤー.
        /// <summary>
        /// プレイヤーの初期速度.
        /// </summary>
        public const float PLAYER_SPEED = 5.0f;

        /// <summary>
        /// プレイヤーの速度上昇倍率.
        /// </summary>
        public const float PLAYER_SPEED_RATE = 0.7f;

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
        public const float PLAYER_MISSILE_SPEED = 4.8f;

        /// <summary>
        /// プレイヤーのミサイルが加速した時の弾速.
        /// </summary>
        public const float PLAYER_MISSILE_BOOST = 10f;

        /// <summary>
        /// プレイヤーのミサイルの旋回速度.
        /// </summary>
        public const float PLAYER_MISSILE_ROTATE_SPEED = 60f;

        /// <summary>
        /// プレイヤーのミサイルが消えるまでの時間.
        /// </summary>
        public const float PLAYER_MISSILE_DELETE_TIME = 1.5f;


        public const float PLAYER_MISSILE_LOCK_TIME = 0.5f;

        /// <summary>
        /// ミサイルの発射レート.
        /// </summary>
        public const float PLAYER_MISSILE_RATE = 0.8f;

        /// <summary>
        /// ラピッドミサイルの発射レート.
        /// </summary>
        public const float PLAYER_RAPID_MISSILE_RATE = 0.4f;

        /// <summary>
        /// プレイヤーのバリアの耐久値.
        /// </summary>
        public const int PLAYER_BARRIER_HP = 12;

        /// <summary>
        /// プレイヤーパワーアップナンバー.
        /// </summary>
        public enum POWER_UP_TYPE
        {
            PLAYER_POWER_UP_NONE = 0,
            PLAYER_POWER_UP_TWINSHOT = 1,
            PLAYER_POWER_UP_MISSILE = 2,
            PLAYER_POWER_UP_LASER = 3,
            PLAYER_POWER_UP_HIGHRATEMISSILE = 4,
            PLAYER_POWER_UP_BARRIER = 5,
        }
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
        public const float ENEMY_BULLET_SPEED = 5.0f;

        /// <summary>
        /// エネミーの弾が消えるまでの時間.
        /// </summary>
        public const float ENEMY_BULLET_DELETE = 3f;

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
        public const float ENEMY_IGNORE_RANGE = 1.5f;

        /// <summary>
        /// エネミーのミサイル弾速.
        /// </summary>
        public const float ENEMY_MISSILE_SPEED = 5f;

        /// <summary>
        /// エネミーのミサイル旋回速度.
        /// </summary>
        public const float ENEMY_MISSILE_ROTATE_SPEED = 25f;

        /// <summary>
        /// エネミーのミサイル消滅までの時間.
        /// </summary>
        public const float ENEMY_MISSILE_DELETE_TIME = 2f;

        /// <summary>
        /// エネミーのミサイル耐久値.
        /// </summary>
        public const int ENEMY_MISSILE_HP = 2;

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
        public const int ENEMY_HP_α = 5;

        /// <summary>
        /// スピードアップドロップ率.
        /// </summary>
        public const float DROP_ITEM_α = 0.45f;

        /// <summary>
        /// パワーアップドロップ率.
        /// </summary>
        public const float DROP_ITEM_β = 0.1f;
        #endregion

        #region エネミーβ
        /// <summary>
        /// エネミーβの移動速度.
        /// </summary>
        public const float ENEMY_SPEED_β = 2.6f;

        /// <summary>
        /// エネミーβの攻撃間隔.
        /// </summary>
        public const float ENEMY_FIRE_RATE_β = 1.5f;

        /// <summary>
        /// エネミーβの停止座標
        /// </summary>
        // public const float ENEMY_RANGE_β = 2f;

        /// <summary>
        /// エネミーβの体力.
        /// </summary>
        public const int ENEMY_HP_β = 8;
        #endregion

        #region エネミーHME(HighMobilityExperiment)
        /// <summary>
        /// エネミーHMEの移動速度.
        /// </summary>
        public const float ENEMY_SPEED_HME = 5.4f;

        /// <summary>
        /// エネミーHMEの横移動速度.
        /// </summary>
        public const float ENEMY_SIDE_SPEED_HME = 4.5f;

        /// <summary>
        /// エネミーHMEの攻撃間隔.
        /// </summary>
        public const float ENEMY_FIRE_RATE_HME = 1.5f;

        /// <summary>
        /// エネミーHMEの弾速.
        /// </summary>
        public const float ENEMY_HME_BULLET_SPEED = 6.5f;

        /// <summary>
        /// エネミーHMEのミサイル弾速.
        /// </summary>
        public const float ENEMY_HME_MISSILE_SPEED = 8f;

        /// <summary>
        /// エネミーHMEのミサイル旋回速度.
        /// </summary>
        public const float ENEMY_HME_MISSILE_ROTATE_SPEED = 45f;

        /// <summary>
        /// エネミーHMEの体力.
        /// </summary>
        public const int ENEMY_HME_HP = 100;

        /// <summary>
        /// エネミーHME出現分岐得点.
        /// </summary>
        public const int ENEMY_HME_POPBORDER = 10000;
        #endregion

        #region ボスエネミー
        /// <summary>
        /// ボスエネミーの移動速度.
        /// </summary>
        public const float ENEMY_BOSS_SPEED = 3f;

        /// <summary>
        /// ボスエネミーの上下移動時間.
        /// </summary>
        public const float ENEMY_BOSS_UP_DOWN_TIME = 0.2f;

        /// <summary>
        /// ボスエネミーの上下移動停止時間.
        /// </summary>
        public const float ENEMY_BOSS_UP_DOWN_STOP_TIME = 2.5f;

        /// <summary>
        /// ボスエネミーの最長攻撃間隔.
        /// </summary>
        public const float ENEMY_BOSS_FIRE_RND_MAX = 2f;
        
        /// <summary>
        /// ボスエネミーの最短攻撃間隔.
        /// </summary>
        public const float ENEMY_BOSS_FIRE_RND_MIN = 1f;

        /// <summary>
        /// ボスエネミーの攻撃パターン変化までの最小攻撃回数.
        /// </summary>
        public const int ENEMY_BOSS_ATTACK_RND_MIN = 1;

        /// <summary>
        /// ボスエネミーの攻撃パターン変化までの最大攻撃回数.
        /// </summary>
        public const int ENEMY_BOSS_ATTACK_RND_MAX = 10;

        /// <summary>
        /// ボスエネミーの体力.
        /// </summary>
        public const int ENEMY_BOSS_HP = 400;

        public enum UpDownState
        {
            Up, StopUp, Down, StopDown
        }
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

    static class Tags
    {
        #region ゲームオブジェクトタグ.
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

        /// <summary>
        /// プレイヤーバリアタグ.
        /// </summary>
        public const string PLAYER_BARRIER = "Player_Barrier";

        /// <summary>
        /// エネミー生成位置指定タグ.
        /// </summary>
        public const string POP_ANCHOR = "PopAnchor";
        #endregion
    }

    public static class Bullets
    {
        #region 生成ナンバー.
        public enum B_Type
        {
            PLAYER_BULLET = 0,
            PLAYER_LASER = 1,
            PLAYER_MISSILE = 2,
            ENEMY_BULLET = 3,
            ENEMY_BULLET_LOCK = 4,
            ENEMY_MISSILE = 5,
        }
        #endregion

        #region 初期生成数.
        /// <summary>
        /// プレイヤー通常弾初期生成数.
        /// </summary>
        public const int PLAYER_BULLET_MAX_COUNT = 40;
        /// <summary>
        /// プレイヤーレーザー弾初期生成数.
        /// </summary>
        public const int PLAYER_LASER_MAX_COUNT = 40;
        /// <summary>
        /// プレイヤーミサイル初期生成数.
        /// </summary>
        public const int PLAYER_MISSILE_MAX_COUNT = 8;
        /// <summary>
        /// エネミー通常弾初期生成数.
        /// </summary>
        public const int ENEMY_BULLET_MAX_COUNT = 25;
        /// <summary>
        /// エネミーロックオン弾初期生成数.
        /// </summary>
        public const int ENEMY_BULLET_LOCK_MAX_COUNT = 25;
        /// <summary>
        /// エネミーミサイル初期生成数.
        /// </summary>
        public const int ENEMY_MISSILE_MAX_COUNT = 20;
        #endregion
    }

    public static class ENum
    {
        #region 生成ナンバー.
        /// <summary>
        /// エネミーの生成指定番号.
        /// </summary>
        public enum E_Type
        {
            ENEMY_NORMAL = 0,
            ENEMY_α = 1,
            ENEMY_β = 2,
            ENEMY_HME = 3,
            ENEMY_BOSS = 4,
            CARRIER = 5,
        }

        public enum AnchorType
        {
            LEFTMOST,
            LEFT4,
            LEFT3,
            LEFT2,
            LEFT1,
            CENTER,
            RIGHT1,
            RIGHT2,
            RIGHT3,
            RIGHT4,
            RIGHTMOST
        }
        #endregion

        #region 初期生成数.
        /// <summary>
        /// エネミーの生成数.
        /// </summary>
        public const int ENEMY_MAX_COUNT = 5;

        /// <summary>
        /// エネミーαの生成数.
        /// </summary>
        public const int ENEMY_α_MAX_COUNT = 4;

        /// <summary>
        /// エネミーβの生成数.
        /// </summary>
        public const int ENEMY_β_MAX_COUNT = 6;

        /// <summary>
        /// エネミーHME(High-Mobility-Experiment)の生成数.
        /// </summary>
        public const int ENEMY_HME_MAX_COUNT = 1;

        /// <summary>
        /// エネミーボスの生成数.
        /// </summary>
        public const int ENEMY_BOSS_MAX_COUNT = 1;

        /// <summary>
        /// アイテムキャリアーの生成数.
        /// </summary>
        public const int CARRIER_MAX_COUNT = 3;
        #endregion
    }

    public static class Effects
    {
        /// <summary>
        /// エフェクト管理番号.
        /// </summary>
        public enum Effect_Type
        {
            EFFECT_EXPLOSION = 0,

            EFFECT_EXPLOSION_MIN = 1,
        }

        #region アニメーションクリップ.
        public const string EXPLOSION_CLIP_NAME = "Explosion";

        public const string EXPLOSION_MIN_CLIP_NAME = "Explosion_Min";
        #endregion

        #region 初期生成数.
        /// <summary>
        /// 爆発エフェクト初期生成数.
        /// </summary>
        public const int EXPLOSION_MAX_COUNT = 15;

        /// <summary>
        /// 小爆発エフェクト初期生成数.
        /// </summary>
        public const int EXPLOSION_MIN_MAX_COUNT = 45;
        #endregion
    }

    public class Score
    {
        /// <summary>
        /// ランキング.
        /// </summary>
        public enum Snum
        {
            RANK_1 = 0,

            RANK_2 = 1,

            RANK_3 = 2,

            RANK_MAX = 3,
        }

        public const int DEFAULT_HIGHSCORE = 10000;

        public const int DEFAULT_MIDDLESCORE = 5000;

        public const int DEFAULT_LOWSCORE = 2000;

        // エネミーの撃破スコア.
        public const int SCORE_ENEMY = 100;

        public const int SCORE_ENEMY_α = 200;

        public const int SCORE_ENEMY_β = 250;

        public const int SCORE_ENEMY_HME = 3000;

        public const int SCORE_ENEMY_BOSS = 2000;

        /// <summary>
        /// パワーアップ余剰分のボーナス
        /// </summary>
        public const int SCORE_BONUS_A = 1000;

        /// <summary>
        /// キル率ボーナス.
        /// </summary>
        public const int SCORE_BONUS_B = 3000;

        /// <summary>
        /// キル数×ボーナススコア.
        /// </summary>
        public const int SCORE_BONUS_KILL = 50;

        /// <summary>
        /// 残りHP×ボーナススコア.
        /// </summary>
        public const int SCORE_BONUS_HP = 1500;

        /// <summary>
        /// ノーダメージボーナス.
        /// </summary>
        public const int SCORE_BONUS_SECRET = 10000;

        /// <summary>
        /// 出現した敵の撃破率でボーナスを出すかの閾値.
        /// </summary>
        public const float SCORE_BONUS_KILL_PERCENT = 0.95f;
    }

    public class Scenes
    {
        #region 遷移先のシーン名.
        public const string TITLE = "TitleScene";

        public const string GAME = "GameScene";

        public const string RESULT = "ResultScene";

        public const string GAMEOVER = "GameOverScene";
        #endregion

        #region 遷移時間.
        /// <summary>
        /// ボス撃破後シーン遷移までの演出時間.
        /// </summary>
        public const float BOSS_DESTROY_TIME = 2f;

        public const float BOSS_DESTROY_WAIT = 0.3f;
        #endregion
    }
}
