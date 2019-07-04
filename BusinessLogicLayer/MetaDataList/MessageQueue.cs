using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;

using DataTransferObject;
using Global.Class.Library;

namespace BusinessLogicLayer
{
    public class MessageQueue
    {

        private string messageQueueName;

        private int noSubQueue;     //No row
        private int noItemPerSubQueue;

        private bool repeatedSubQueueIndex;
        private bool invertSubQueueIndex;

        private int noItemAdded;        //total item added, use this for Enqueue Method to add item for each sub queue

        private List<List<MessageData>> messageSubQueue;  //Important note: List use zero based index

        #region Contructor

        public MessageQueue()
        {
            this.NoSubQueue = GlobalVariables.NoSubQueue();
            this.NoItemPerSubQueue = this.NoItemPerCarton / this.NoSubQueue;

            this.RepeatedSubQueueIndex = false;
            this.invertSubQueueIndex = false;

            this.noItemAdded = 0; //Inititalize
        }

        /// <summary>
        /// ONLY matchingPackList USE THIS CONTRUCTOR. This contructor beside allow to set NoItemPerSubQueue, it is also allow to set RepeatedSubQueueIndex
        /// </summary>
        /// <param name="noItemPerSubQueue"></param>
        public MessageQueue(int noItemPerSubQueue)
            : this()
        {
            this.NoItemPerSubQueue = noItemPerSubQueue;
            this.RepeatedSubQueueIndex = GlobalVariables.RepeatedSubQueueIndex();

            if (this.RepeatedSubQueueIndex) this.noItemAdded = 0 - this.NoItemPerSubQueue;
        }


        #endregion Contructor

        #region Public Properties

        public string MessageQueueName
        {
            get
            {
                return this.messageQueueName;
            }

            set
            {
                if (value != this.messageQueueName)
                {
                    this.messageQueueName = value;
                }
            }
        }


        public int NoSubQueue
        {
            get
            {
                return this.noSubQueue;
            }
            protected set
            {
                if (this.noSubQueue != value)
                {
                    this.noSubQueue = value;

                    this.messageSubQueue = new List<List<MessageData>>();
                    for (int i = 1; i <= this.NoSubQueue; i++)
                    {
                        this.messageSubQueue.Add(new List<MessageData>());
                    }
                }
            }

        }


        public int NoItemPerCarton
        {
            get
            {
                return GlobalVariables.NoItemPerCarton(); ;
            }
        }

        private int NoItemPerSubQueue
        {
            get
            {
                return this.noItemPerSubQueue;
            }
            set
            {
                this.noItemPerSubQueue = value;
            }
        }


        private bool RepeatedSubQueueIndex
        {
            get
            {
                return this.repeatedSubQueueIndex;
            }
            set
            {
                this.repeatedSubQueueIndex = value;
            }
        }

        /// <summary>
        /// Return the total number of items in MessageQueue
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;

                foreach (List<MessageData> subQueue in this.MessageSubQueue)
                {
                    count = count + subQueue.Count;
                }

                return count;
            }
        }

        /// <summary>
        /// Return the number of items of a specific subQueueID
        /// </summary>
        public int GetSubQueueCount(int subQueueID)
        {
            return this.MessageSubQueue[subQueueID].Count;
        }

        private List<List<MessageData>> MessageSubQueue
        {
            get { return this.messageSubQueue; }
        }

        #endregion Public Properties

        #region Public Method

        /// <summary>
        /// The SubQueueID of Next Pack when Enqueue
        /// </summary>
        public int NextPackSubQueueID
        {       //Sequence Enqueue to each sub queue, this line will return: index : 0, 1, 3, ... NoSubQueue-1 (Comfort with: Zero base index)
            get
            {
                if (!this.RepeatedSubQueueIndex)
                    return (this.noItemAdded / this.NoItemPerSubQueue) % this.NoSubQueue;
                else
                {
                    int nextPackSubQueueID = this.noItemAdded < 0 ? 0 : (this.noItemAdded / this.NoItemPerSubQueue) % this.NoSubQueue;
                    if (this.invertSubQueueIndex) nextPackSubQueueID = (this.NoSubQueue - 1) - nextPackSubQueueID;

                    return nextPackSubQueueID;
                }
            }
        }

        /// <summary>
        /// Add messageData by specific messageData.PackSubQueueID, without increase noItemAdded by 1
        /// This is used to initialize the Queue
        /// </summary>
        /// <param name="messageData"></param>
        /// <param name="packSubQueueID"></param>
        public void AddPack(MessageData messageData)
        {
            if (messageData.PackSubQueueID < this.MessageSubQueue.Count)
                this.MessageSubQueue[messageData.PackSubQueueID].Add(messageData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageData"></param>
        public void Enqueue(MessageData messageData)
        {
            this.MessageSubQueue[this.NextPackSubQueueID].Add(messageData);
            this.noItemAdded++; //Note: Always increase noItemAdded by 1 after Enqueue

            if (this.RepeatedSubQueueIndex && this.noItemAdded > 0 && (this.noItemAdded % (this.NoSubQueue * this.NoItemPerSubQueue) == 0)) this.invertSubQueueIndex = !this.invertSubQueueIndex;

        }


        /// <summary>
        /// Dequeue a batch of noItemPerCarton of elements from this Matching Queue, by sequence Dequeue from each sub queue, with index 0, 1, 2, 3, ... NoSubQueue-1 (Comfort with: Zero base index)
        /// </summary>
        /// <returns></returns>
        public MessageQueue DequeuePack()
        {
            MessageQueue packInOneCarton = new MessageQueue();


            foreach (List<MessageData> subQueue in this.MessageSubQueue)
            {
                if (packInOneCarton.NoItemPerSubQueue > subQueue.Count) return packInOneCarton; //There is not enough element in this sub queue to dequeue. In this case, return empty
            }


            foreach (List<MessageData> subQueue in this.MessageSubQueue)
            {
                for (int i = 0; i < packInOneCarton.NoItemPerSubQueue; i++)
                {
                    if (subQueue.Count > 0) { packInOneCarton.Enqueue(subQueue.ElementAt(0)); subQueue.RemoveAt(0); }//Check subQueue.Count > 0 just for sure, however, we check it already at the begining of this method
                }
            }

            return packInOneCarton;
        }

        public MessageData Dequeue(int packID)
        {
            foreach (List<MessageData> subQueue in this.MessageSubQueue)
            {
                foreach (MessageData packData in subQueue)
                {
                    if (packData.PackID == packID)
                    {
                        MessageData messageData = packData.ShallowClone();
                        subQueue.Remove(packData);

                        return messageData;
                    }
                }
            }
            return null; //Return null if can not find any PackID
        }



        /// <summary>
        /// messageData.PackSubQueueID: Will change to new value (new position) after replace
        /// </summary>
        /// <param name="packID"></param>
        /// <param name="messageData"></param>
        /// <returns></returns>
        public bool Replace(int packID, MessageData messageData)
        {
            foreach (List<MessageData> subQueue in this.MessageSubQueue)
            {
                for (int i = 0; i < subQueue.Count; i++)
                {
                    if (subQueue[i].PackID == packID)
                    {
                        messageData.PackSubQueueID = subQueue[i].PackSubQueueID;
                        subQueue[i] = messageData;
                        return true;
                    }
                }
            }
            return false; //Return false if can not find any PackID
        }


        public virtual DataTable GetAllElements()
        {
            int maxSubQueueCount = 0;
            DataTable dataTableAllElements = new DataTable("AllElements");

            foreach (List<MessageData> subQueue in this.MessageSubQueue)
            {
                maxSubQueueCount = maxSubQueueCount <= subQueue.Count ? subQueue.Count : maxSubQueueCount;
            }

            for (int i = 0; i < maxSubQueueCount; i++)//Make a table with number of column equal to maxSubQueueCount
            {
                dataTableAllElements.Columns.Add((i < 9 ? " " : "") + (i + 1).ToString().Trim());
            }

            foreach (List<MessageData> subQueue in this.MessageSubQueue)
            {
                DataRow dataRow = dataTableAllElements.NewRow(); //add row for each sub queue
                for (int i = 0; i < maxSubQueueCount; i++)
                {//Zero base queue element
                    if (subQueue.Count > i) dataRow[i] = subQueue.ElementAt<MessageData>(i).PrintedBarcode + GlobalVariables.doubleTabChar + GlobalVariables.doubleTabChar + subQueue.ElementAt<MessageData>(i).PackID; //Fill data row
                }
                dataTableAllElements.Rows.Add(dataRow);
            }

            return dataTableAllElements;

        }

        /// <summary>
        /// Get element at index of whole queue. Zero base index. Return Null if index out of range
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MessageData ElementAt(int index)
        {
            if (index < this.Count)  //Zero base index
            {
                int findIndex = -1;
                foreach (List<MessageData> subQueue in this.MessageSubQueue)
                {
                    for (int i = 0; i < subQueue.Count; i++)
                    {
                        findIndex++;
                        if (findIndex == index) return subQueue.ElementAt(i);
                    }
                }
            }
            return null; //Return Null if index out of range
        }

        /// <summary>
        /// Get element at index of subQueueID. Zero base index. Return Null if index out of range
        /// </summary>
        /// <param name="subQueueID"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public MessageData ElementAt(int subQueueID, int index)
        {
            if (subQueueID >= 0 && subQueueID < this.NoSubQueue && index >= 0 && index < this.MessageSubQueue[subQueueID].Count)  //Zero base index
            {
                return this.MessageSubQueue[subQueueID].ElementAt(index);
            }
            return null; //Return Null if index out of range
        }

        #endregion Public Method
    }
}
