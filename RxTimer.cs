using System;
using UniRx;

/// <summary>
/// 簡単なUniRxを使ったタイマー
/// </summary>
public class RxTimer
{
    //外部にタイマーを公開
    public IObservable<int> CountDownObservable
    {
        get
        {
            return countDownObservable.AsObservable();
        }
    }

    //タイマーを停止するのに使用
    private Subject<Unit> stopSubject = new Subject<Unit>();
    //Hot変換に使用
    private IConnectableObservable<int> countDownObservable;

    /// <summary>
    /// タイマーを作成
    /// </summary>
    /// <param name="time">カウントする秒数</param>
    public void CreateTimer(int time)
    {
        countDownObservable = CreateCountDownObservable(time).Publish();
    }

    /// <summary>
    /// 作成したタイマーを実行する
    /// </summary>
    public void StartTimer()
    {
        countDownObservable.Connect();
    }

    /// <summary>
    /// 実行しているタイマーを停止させる
    /// </summary>
    public void StopTimer()
    {
        stopSubject.OnNext(Unit.Default);
    }

    /// <summary>
    /// タイマー本体
    /// 指定秒数カウントする
    /// </summary>
    /// <param name="countTime">秒数</param>
    /// <returns></returns>
    private IObservable<int> CreateCountDownObservable(int countTime)
    {
        return Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            .Select(x => (int)(countTime - x))
            .TakeUntil(stopSubject)
            .TakeWhile(x => x > 0);
    }
}
