from Crypto.Cipher import AES
import base64

# 定義一個異常，用於當找到密鑰時終止循環
class FoundKeyException(Exception):
    pass

# 已知的密文和部分金鑰
cipher_text = "INNkAZHIpe5u9LvzhH24VyORcZQVDCFXzV6V/l9M7rpgqskMxvaRbGwR2dZaxMDZ"
partial_key = "0lOS▆b]▆&N)▆w▆@+"

# 已知的明文和密文
known_plaintext = b"security"
known_ciphertext_b64 = "pKjVPv28yVMn5cRXeUNYpg=="
known_ciphertext = base64.b64decode(known_ciphertext_b64)

# 將密文從base64格式解碼
cipher_text_bytes = base64.b64decode(cipher_text)

# 找到部分金鑰中未知字符的位置
unknown_positions = [pos for pos, char in enumerate(partial_key) if char == '▆']

# 嘗試所有可能的字符組合
try:  
    for i in range(33, 127):
        for j in range(33, 127):
            for k in range(33, 127):
                for l in range(33, 127):
                    try_key = partial_key.replace("▆", chr(i), 1).replace("▆", chr(j), 1).replace("▆", chr(k), 1).replace("▆", chr(l), 1)
                    
                    try:
                        cipher = AES.new(try_key.encode('utf-8'), AES.MODE_ECB)
                        decrypted_text = cipher.decrypt(cipher_text_bytes)
                        
                        # 檢查解密後的文本是否符合預期
                        if cipher.decrypt(known_ciphertext).strip() == known_plaintext:
                            print(f"Success, Key: {try_key}, Decrypted text: {decrypted_text}")
                            raise FoundKeyException
                        else :
                            print(f"Fail : {chr(i)}, {chr(j)}, {chr(k)}, {chr(l)}")
                    except Exception as e:
                        pass  
except FoundKeyException:
    print("Found the key and decrypted the message!")
