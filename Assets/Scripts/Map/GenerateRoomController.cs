using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GenerateRoom
{
    public class RoomGenerateManager : MonoBehaviour
    {
        public RoomItem StartRoomItem;
        public RoomItem NormalRoomItem;
        public RoomItem ShopRoomItem;
        public RoomItem BossRoomItem;
        
        
        public LineRenderer Line;
        public DynaGrid<RoomItem> Grid = new DynaGrid<RoomItem>();

        private void Start()
        {
            var startRoom = new RoomConfig().StartRoom;
            GenerateRoomByDfs(startRoom);
            
            Grid.ForEach(room =>
            {
                DrawLine(room);
            });
        }
        //深度优先遍历生成地图数据
        void GenerateRoomByDfs(RoomNode startRoom,int level = 0)
        {
            if (Grid[startRoom.X, startRoom.Y] == null)
            {
                var room = Instantiate(GetRoomItemByType(startRoom));
                Grid[startRoom.X, startRoom.Y] = room;
                room.Node = startRoom;
                room.transform.position = new Vector3(startRoom.X * 2, 2 * level);
                room.gameObject.SetActive(true);
            }
            
            foreach (var startRoomNextRoom in startRoom.NextRooms)
            {
                GenerateRoomByDfs(startRoomNextRoom, level + 1);
            }
        }
        
        RoomItem GetRoomItemByType(RoomNode roomNode)
        {
            if (roomNode.Type == RoomType.Normal)
            {
                return NormalRoomItem;
            }
            else if (roomNode.Type == RoomType.Start)
            {
                return StartRoomItem;
            }
            else if (roomNode.Type == RoomType.Shop)
            {
                return ShopRoomItem;
            }
            else if (roomNode.Type == RoomType.Boss)
            {
                return BossRoomItem;
            }

            return null;
        }
        
        void DrawLine(RoomItem roomItem)
        {
            foreach (var nodeNextRoom in roomItem.Node.NextRooms)
            {
                var line = Instantiate(Line);
                line.SetPosition(0,roomItem.transform.position);
                line.SetPosition(1,Grid[nodeNextRoom.X,nodeNextRoom.Y].transform.position);
                line.gameObject.SetActive(true);
            }
        }
    }
    public class DynaGrid<T>
    {
        private Dictionary<Tuple<int, int>, T> mGrid = null;

        public DynaGrid()
        {
            mGrid = new Dictionary<Tuple<int, int>, T>();
        }

        public void ForEach(Action<int, int, T> each)
        {
            foreach (var kvp in mGrid)
            {
                each(kvp.Key.Item1, kvp.Key.Item2, kvp.Value);
            }
        }

        public void ForEach(Action<T> each)
        {
            foreach (var kvp in mGrid)
            {
                each(kvp.Value);
            }
        }
        
        public T this[int xIndex, int yIndex]
        {
            get
            {
                var key = new Tuple<int, int>(xIndex, yIndex);
                return mGrid.TryGetValue(key, out var value) ? value : default;
            }
            set
            {
                var key = new Tuple<int, int>(xIndex, yIndex);
                mGrid[key] = value;
            }
        }

        public void Clear(Action<T> cleanupItem = null)
        {
            mGrid.Clear();
        }
    }
}
