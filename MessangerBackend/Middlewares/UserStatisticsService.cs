public class UserStatisticsService
{
    private readonly IDictionary<string, int> _statistics = new Dictionary<string, int>();

    public IDictionary<string, int> GetStatistics()
    {
        return _statistics;
    }

    public void IncrementCount(string nickname)
    {
        if (_statistics.ContainsKey(nickname))
        {
            _statistics[nickname]++;
        }
        else
        {
            _statistics[nickname] = 1;
        }
    }
}
