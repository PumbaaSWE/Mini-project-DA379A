using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <para>  Singleton instance that keeps track of deliveries.  </para>
/// <para>  Use Instance() for EZ access.                       </para>
/// 
/// </summary>
public class Deliveries
{
    private static Deliveries instance;
    private Deliveries()
    {
        waitingRecipients = new List<Recipient>();
    }

    public static Deliveries Instance()
    {
        instance ??= new Deliveries();

        return instance;
    }

    private const int deliveriesToComplete = 1;

    private List<Recipient> waitingRecipients;
    private Recipient currentRecipient;
    private int completedDeliveries;

    /// <summary>
    /// 
    /// Request delivery to <paramref name="recipient"/>.
    /// 
    /// </summary>
    public void RequestDelivery(Recipient recipient)
    {
        if (waitingRecipients.Contains(recipient))
        {
            Debug.LogError("This recipient has already made a request.");
            return;
        }

        waitingRecipients.Add(recipient);
    }

    /// <summary>
    /// 
    /// Start a randomly selected delivery from available recipients.
    /// 
    /// </summary>
    public void StartRandomDelivery()
    {
        if (currentRecipient != null)
        {
            Debug.LogError("A delivery has already been started.");
            return;
        }

        int randomIndex = Random.Range(0, waitingRecipients.Count);
        currentRecipient = waitingRecipients[randomIndex];
        waitingRecipients.RemoveAt(randomIndex);

        Debug.Log("Delivery package to " + currentRecipient.name);
    }

    /// <summary>
    /// 
    /// Mark delivery to <paramref name="recipient"/> as complete.
    /// If there are any active delivery requests, a new delivery will be started immediately.
    /// 
    /// </summary>
    public void CompleteDelivery(Recipient recipient)
    {
        if (recipient != currentRecipient)
        {
            Debug.LogError("Delivery to this recipient hasn't started yet.");
            return;
        }

        currentRecipient = null;
        completedDeliveries++;

        if (completedDeliveries < deliveriesToComplete)
        {
            StartRandomDelivery();
        }
    }

    /// <summary>
    /// 
    /// Returns the <paramref name="recipient"/> that is waiting for our delivery.
    /// 
    /// </summary>
    public Recipient CurrentDelivery()
    {
        return currentRecipient;
    }

    /// <summary>
    /// 
    /// Returns <paramref name="true"/> if the player has completed enough deliveries.
    /// 
    /// </summary>
    public bool Finished()
    {
        return completedDeliveries >= deliveriesToComplete;
    }

    public int Completed()
    {
        return completedDeliveries;
    }

    public int Needed()
    {
        return deliveriesToComplete;
    }

    public int Waiting()
    {
        return waitingRecipients.Count;
    }
}
