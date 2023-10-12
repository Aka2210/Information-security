from Crypto.Cipher import AES
import base64

# 已知的密文和金鑰
cipher_text_b64 = "pKjVPv28yVMn5cRXeUNYpg=="
key = "123456789"

# 將密文從base64格式解碼
cipher_text_bytes = base64.b64decode(cipher_text_b64)

# 由於AES需要固定長度的金鑰，我們可能需要將金鑰擴展到合適的長度
# 在這個例子中，我們將金鑰擴展到16字節（128位）
key = key.ljust(16, '0')

# 創建一個AES cipher對象
cipher = AES.new(key.encode('utf-8'), AES.MODE_ECB)

# 解密
decrypted_text_bytes = cipher.decrypt(cipher_text_bytes)

# 將解密後的字節轉換為十六進制，以便於閱讀
decrypted_text_hex = decrypted_text_bytes.hex()

# 輸出解密後的字節
print("Decrypted text (hex):", decrypted_text_hex)
