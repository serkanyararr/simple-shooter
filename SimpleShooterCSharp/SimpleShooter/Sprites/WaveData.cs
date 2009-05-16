namespace SimpleShooter.Sprites
{
    public class WaveData
    {
        public WaveData(int count, double fireRate, int atOnce, int fireatonce)
        {
            EnemyCount = count;
            fireRateMilliseconds = fireRate;
            enemiesAtOnce = atOnce;
            fireAtOnce = fireatonce;
            waveEmpty = false;
        }
        public int EnemyCount { get; set; }
        public double fireRateMilliseconds { get; set; }
        public int enemiesAtOnce { get; set; }
        public int fireAtOnce { get; set; }
        public bool waveEmpty { get; set; }
    }
}
