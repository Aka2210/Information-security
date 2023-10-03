#include<iostream>
#include<string>
#include<math.h>
#include<queue>
#include<cstdlib>
#include<ctime>
#include<cctype>

using namespace std;

struct TreeNode
{
    char val;
    TreeNode *left;
    TreeNode *right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(char x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(char x, TreeNode *left, TreeNode *right) : val(x), left(left), right(right) {}
};

void SequentialReplacement(TreeNode* node, string& curr)
{
    if(node == NULL)
        return;

    curr += node->val;
    SequentialReplacement(node->left, curr);
    SequentialReplacement(node->right, curr);
    return;
}

TreeNode* buildTree(string& s, int& index, int curr) {
    if (curr >= s.size()) {
        index--;
        return nullptr;
    }

    TreeNode* node = new TreeNode(s[index]);

    index++;
    node->left = buildTree(s, index, 2 * curr + 1);

    index++;
    node->right = buildTree(s, index, 2 * curr + 2);


    return node;
}

string Encode(string plaintext, string& key)
{
    TreeNode *root = new TreeNode();
    TreeNode *node = root;
    string Ciphertext = "";
    srand(time(0));

    string temp = "";
    for(int i = 0; i < plaintext.size(); i++)
    {
        int judge = (int)plaintext[i] + 15 + i;
        if(isupper(plaintext[i]))
        {
            key += (65 + rand() % 26);
            while(judge > 'Z')
                judge = 'A' + (judge - 'Z') - 1;
        }
        else
        {
            key += (97 + rand() % 26);
            while(judge > 'z')
                judge = 'a' + (judge - 'z') - 1;
        }
        
        temp += toupper((char)judge);
    }
    plaintext = temp;

    queue<TreeNode*> q;
    q.push(root);
    root->val = plaintext[0];
    int index = 1;

    while(!q.empty())
    {
        int count = q.size();

        for(int i = 0; i < count; i++)
        {
            if(index < plaintext.size())
            {
                q.front()->left = new TreeNode(plaintext[index]);
                q.push(q.front()->left);
                index++;
            }

            if(index < plaintext.size())
            {
                q.front()->right = new TreeNode(plaintext[index]);
                q.push(q.front()->right);
                index++;
            }

            q.pop();
        }
    }

    SequentialReplacement(root, Ciphertext);
    return Ciphertext;
}

string Uncode(string Ciphertext, string key)
{
    int index = 0;
    TreeNode* root = buildTree(Ciphertext, index, 0);
    string plaintext = "";
    queue<TreeNode*> q;
    q.push(root);
    index = 0;

    while(!q.empty())
    {
        int count = q.size();
        for(int i = 0; i < count; i++)
        {
            if(index < Ciphertext.size())
            {
                int judge = (int)q.front()->val - 15 - index;
                while(judge < 'A')
                    judge = 'Z' - ('A' - judge) + 1;
                if(isupper(key[index]))
                    plaintext += (char)judge;
                else
                    plaintext += tolower((char)judge);
                index++;
            }

            if(q.front()->left != NULL)
                q.push(q.front()->left);
            
            if(q.front()->right != NULL)
                q.push(q.front()->right);

            q.pop();
        }
    }
    return plaintext;
}

int main()
{
    string operate;
    cout << "Enter Encode for encryption and Uncode for decryption: ";
    getline(cin, operate);

    if(operate == "Encode")
    {
        cout << "Please enter the plain text to be encrypted:";
        string plaintext, key = "";
        getline(cin, plaintext);
        string Ciphertext = Encode(plaintext, key);
        cout << "Ciphertext:" << Ciphertext << "\nkey:" << key;
    }
    else if(operate == "Uncode")
    {  
        cout << "Please enter the cipher text to be decoded:";
        string Ciphertext;
        getline(cin, Ciphertext);
        cout << "Please enter the key corresponding to the ciphertext:";
        string key;
        getline(cin, key);
        cout << "plaintext: " << Uncode(Ciphertext, key);
    }
}