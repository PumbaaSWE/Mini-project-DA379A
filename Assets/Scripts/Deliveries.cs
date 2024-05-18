using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <para>  Singleton instance that keeps track of deliveries.  </para>
/// <para>  Use GetInstance() for easy access.                       </para>
/// 
/// </summary>
public class Deliveries : GenericSingleton<Deliveries>
{
    private readonly List<Recipient> waitingRecipients;

    private Recipient currentRecipient;
    private Recipient lastRecipient;

    private Deliveries() : base()
    {
        waitingRecipients = new List<Recipient>();
    }

    /// <summary>
    /// 
    /// Request delivery to <paramref name="recipient"/>.
    /// 
    /// </summary>
    public void AddRecipient(Recipient recipient)
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

        while(currentRecipient == null)
        {
            int randomIndex = Random.Range(0, waitingRecipients.Count);

            if (waitingRecipients[randomIndex] != lastRecipient)
            {
                currentRecipient = waitingRecipients[randomIndex];
            }
        }
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

        lastRecipient = currentRecipient;
        currentRecipient = null;

        StartRandomDelivery();
    }

    /// <summary>
    /// 
    /// Returns the <paramref name="recipient"/> that is waiting for our delivery.
    /// 
    /// </summary>
    public Recipient CurrentRecipient()
    {
        return currentRecipient;
    }

    public int Waiting()
    {
        return waitingRecipients.Count;
    }
}
