using System;

public class RandomNumber 
{
    private static RandomNumber m_instance;
    private Random m_random;

    private RandomNumber(int seed)
    {
        m_random = new Random(seed);
    }

    public static RandomNumber Instance
    {
        get
        {
            if (m_instance == null)
            {
                // Using a seed based on time to ensure different sequences on each run
                int seed = Environment.TickCount;
                m_instance = new RandomNumber(seed);
            }
            return m_instance;
        }
    }

    // Generates a random float between 0.0 and 1.0
    public float NextFloat()
    {
        return (float)m_random.NextDouble();
    }

    // Generates a random float between min (inclusive) and max (inclusive)
    public float NextFloat(float min, float max)
    {
        return min + (float)m_random.NextDouble() * (max - min);
    }

    // Generates a random integer between 0 (inclusive) and max (exclusive)
    public int NextInt(int max)
    {
        return m_random.Next(max);
    }

    // Generates a random integer between min (inclusive) and max (exclusive)
    public int NextInt(int min, int max)
    {
        return m_random.Next(min, max);
    }

    // Generates a random boolean value
    public bool NextBool()
    {
        return m_random.Next(0, 2) == 1;
    }

    // Seeds the RNG with a specific seed
    public void Seed(int seed)
    {
        m_random = new Random(seed);
    }
}
