namespace StarSonata.Api.Messages.Incoming
{
    using System;
    using System.Collections.Generic;

    public class CharacterList : IIncomingMessage
    {
        public CharacterList(ReadOnlySpan<byte> data)
        {
            var characters = new List<Character>();
            var byteOffset = 0;
            var characterCount = ByteUtility.GetShort(data, ref byteOffset);
            for (var i = 0; i < characterCount; ++i)
            {
                if (byteOffset < data.Length)
                {
                    var id = ByteUtility.GetInt(data, ref byteOffset);
                    var name = ByteUtility.GetString(data, ref byteOffset);
                    _ = ByteUtility.GetInt(data, ref byteOffset);
                    _ = ByteUtility.GetInt(data, ref byteOffset);
                    _ = ByteUtility.GetInt(data, ref byteOffset);
                    _ = ByteUtility.GetString(data, ref byteOffset);
                    _ = ByteUtility.GetInt(data, ref byteOffset);

                    if (byteOffset < data.Length)
                    {
                        var hasSkins = ByteUtility.GetBoolean(data, ref byteOffset);
                        if (hasSkins)
                        {
                            var skinCount = ByteUtility.GetInt(data, ref byteOffset);
                            for (var j = 0; j < skinCount; ++j)
                            {
                                _ = ByteUtility.GetByte(data, ref byteOffset);
                                _ = ByteUtility.GetString(data, ref byteOffset);
                                _ = ByteUtility.GetString(data, ref byteOffset);
                                _ = ByteUtility.GetLong(data, ref byteOffset);
                                _ = ByteUtility.GetByte(data, ref byteOffset);
                                for (var k = 0; k < 6; ++k)
                                {
                                    _ = ByteUtility.GetString(data, ref byteOffset);
                                }
                            }
                        }
                    }

                    characters.Add(new Character { Id = id, Name = name });
                }
            }

            ByteUtility.ByteArrayToHexString(data);
            this.Characters = characters.ToArray();
        }

        public IReadOnlyCollection<Character> Characters { get; set; }
    }
}
