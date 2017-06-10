using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace W6
{
    public static class Serializer
    {
        public static byte[] SerializeToBytes<T>(T source)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, source);
                return stream.ToArray();
            }
        }
        public static T DeserializeFromBytes<T>(byte[] source)
        {
            using (var stream = new MemoryStream(source))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

    }
    public static class BinaryConverter
    {
        public static byte[] TilesToBinary(Tiles tiles)
        {
            var tilesIndexList = new List<ushort>();
            var keyStroage = new Dictionary<string, IndexKey>();
            var indexBytes = new List<byte[]>();
            int maxIndex = 0;
            // Tile 바이너리 추출 및 헤더 추가
            for (int n = 0; n < tiles.GetCount(); ++n)
            {
                string name = "defaultTile";
                if (tiles[n] != null && tiles[n].GetSprite() != null)
                    name = tiles[n].GetSprite().name;
                IndexKey key;
                if (!keyStroage.TryGetValue(name, out key))
                {
                    key = new IndexKey() { FileName = name, Index = maxIndex };
                    keyStroage.Add(name, key);
                    maxIndex++;
                }
                tilesIndexList.Add((ushort)key.Index);
            }
            // 헤더 바이너리 추출
            foreach (var indexKey in keyStroage.Values)
                indexBytes.Add(Serializer.SerializeToBytes(indexKey));

            using (var stream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(stream))
                {
                    /// Resource Index 저장
                    // ResourceIndex 갯수 저장( 2 bytes )
                    binaryWriter.Write((ushort)indexBytes.Count);
                    for (int n = 0; n < indexBytes.Count; ++n)
                    {
                        var indexByte = indexBytes[n];
                        // Byte Length 저장 ( 2 bytes )
                        binaryWriter.Write((ushort)indexByte.Length);
                        // Index Binary 저장 ( ? bytes )
                        binaryWriter.Write(indexByte);
                    }
                    // Row , Col, BlockSize ( 2 + 2 + 2 bytes);
                    binaryWriter.Write((ushort)tiles.MaxRow);
                    binaryWriter.Write((ushort)tiles.MaxCol);
                    binaryWriter.Write((ushort)tiles.BlockSize);
                    for (int n = 0; n < tilesIndexList.Count; ++n)
                    {
                        // tile Index 저장 ( 2 bytes );
                        binaryWriter.Write(tilesIndexList[n]);
                    }
                }
                return stream.ToArray();
            }
        }
        public static Tiles BinaryToTiles( byte[] tilesBinary, Tiles targetTiles)
        {
            const string tilePath = "Prefab/Tile/tile";
            var keyStroage = new Dictionary<int, IndexKey>();

            using (var stream = new MemoryStream(tilesBinary))
            {
                using (BinaryReader binaryReader = new BinaryReader(stream))
                {
                    // ResourceIndex 갯수 로드( 2 bytes )
                    int resourceIndexCount = binaryReader.ReadUInt16();
                    IndexKey indexKey;
                    for (int n = 0; n < resourceIndexCount; ++n)
                    {
                        // Byte Length 로드 ( 2 bytes )
                        ushort bufferSize = binaryReader.ReadUInt16();
                        byte[] buffer = new byte[bufferSize];
                        // Index Binary 로드 ( ? bytes )
                        binaryReader.Read(buffer, 0, bufferSize);
                        indexKey = Serializer.DeserializeFromBytes<IndexKey>(buffer);
                        keyStroage.Add(indexKey.Index, indexKey);
                    }
                    // Row , Col ( 2 + 2 bytes);
                    ushort row          = binaryReader.ReadUInt16();
                    ushort col          = binaryReader.ReadUInt16();
                    ushort blockSize    = binaryReader.ReadUInt16();

                    var tileList = targetTiles.ToList();
                    int listIndex = 0;

                    targetTiles = new Tiles( row, col, blockSize);
                    
                    for (int y = 0; y < col; ++y)
                    {
                        for (int x = 0; x < row; ++x)
                        {
                            string fileName = "defaultTile";
                            // TileIndex 2byte
                            
                            if(  keyStroage.TryGetValue( binaryReader.ReadUInt16(),out indexKey) )
                            {
                                fileName = indexKey.FileName;
                            }

                            if(listIndex >= tileList.Count)
                            {
                                GameObject go = Object.Instantiate(Resources.Load( tilePath )) as GameObject;
                               targetTiles[x, y] = go.GetComponent<Tile>();
                            }
                            else
                            {
                                targetTiles[x, y] = tileList[listIndex++];
                            }
                            targetTiles[x, y].transform.position =
                                new Vector3(x * targetTiles.BlockDistance, y * targetTiles.BlockDistance, 0.0f);
                            if (fileName != "defaultTile")
                                targetTiles[x, y].ChangeSprite(SpriteManager.Instance.GetSprite(fileName));
                            else
                                targetTiles[x, y].ChangeSprite(null);
                        }
                    }
                }
                return targetTiles;
            }
        }
    }
}
