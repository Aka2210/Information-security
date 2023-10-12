from Crypto.Cipher import AES
import base64

# 定義一個異常，用於當找到密鑰時終止循環
class FoundKeyException(Exception):
    pass

# 密文和部分金鑰
cipher_text = "INNkAZHIpe5u9LvzhH24VyORcZQVDCFXzV6V/l9M7rpgqskMxvaRbGwR2dZaxMDZ"
partial_key = "0lOS▆b]▆&N)▆w▆@+"

# 將密文從base64格式解碼
cipher_text_bytes = base64.b64decode(cipher_text)

# 預期的明文和密文
expected_plaintext = b"security"
expected_ciphertext = b"pKjVPv28yVMn5cRXeUNYpg=="

try:  
    for i in range(97, 127):
        for j in range(33, 127):
            for k in range(33, 127):
                for l in range(33, 127):
                    try_key = partial_key.replace("▆", chr(i), 1)
                    try_key = try_key.replace("▆", chr(j), 1)
                    try_key = try_key.replace("▆", chr(k), 1)
                    try_key = try_key.replace("▆", chr(l), 1)
                    
                    # 確保密鑰長度為16的倍數
                    try_key = try_key.ljust(32, '0')
                    
                    try:
                        cipher = AES.new(try_key.encode('utf-8'), AES.MODE_ECB)
                        decrypted_text = cipher.decrypt(cipher_text_bytes)
                        
                        # 檢查解密後的文本是否符合預期
                        if decrypted_text.strip() == expected_plaintext:
                            print(f"Success, Key: {try_key}, Decrypted text: {decrypted_text}")
                            raise FoundKeyException
                        else :
                            print(f"Fail : {i}, {j}, {k}, {l}")
                    except Exception as e:
                        pass  
except FoundKeyException:
    print("Found the key and decrypted the message!")
