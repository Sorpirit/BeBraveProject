using System;
using Core.CardSystem.Data;
using Core.RoomsSystem;
using UnityEngine;

namespace Game.RoomFactories
{
    public interface IFactoryAggregator
    {
        /// <summary>
        /// Adds factory for defined type
        /// </summary>
        /// <param name="factory"></param>
        /// <typeparam name="T"></typeparam>
        void AddFactory<T>(IRoomContentFactory<T> factory) where T : ICardDescription;
        void AddFactory<T>(Func<T, GameObject, IRoomContent> factory) where T : ICardDescription;
    }
}