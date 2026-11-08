#include<bits/stdc++.h>
using namespace std;
#define int long long
#define vi  vector<int>
#define vvi vector<vector<int>>
#define pb push_back
#define sz(x) x.size()
#define all(x) x.begin(),x.end()
#define rep(x,a,b) for(int x=a;x!=b;x = x + (b>a ? 1 : -1))

struct ListNode
{
    int val;
    ListNode *next;
    ListNode *child;
    ListNode()
    {
        val = 0;
        next = NULL;
        child = NULL;
    }
    ListNode(int data1)
    {
        val = data1;
        next = NULL;
        child = NULL;
    }
    ListNode(int data1, ListNode *next1, ListNode* next2)
    {
        val = data1;
        next = next1;
        child = next1;
    }
};
 

ListNode* flattenLinkedList(ListNode* &head)
{
	if(head == nullptr) return head;
	if(!head->next && !head->child) return head;

	ListNode* nextNode = head->next, *childNode = head->child;
	if(head->next)
	{
		flattenLinkedList(head->next);
		head->next = nullptr;
	}	

	if(head->child)
		flattenLinkedList(head->child);

	head->child = mergeLinkedLists(nextNode, childNode);
}

ListNode* mergeLinkedLists(ListNode* l1, ListNode* l2)
{
	if(!l1 && !l2) return nullptr;
	if(!l1) return l2;
	if(!l2) return l1;

	ListNode* head = l1->val <= l2->val ? l1 : l2;
	ListNode* curr = nullptr
	while(l1 || l2)
	{
		if(!l1)
		{
			curr->next = l2;
			break;
		}

		if(!l2)
		{
			curr->next = l1;
			break;
		}

		if(l1->val <= l2->val)
		{
			curr->next = l1;
			curr = curr->next;
			l1 = l1->next;
		}
		else
		{
			curr->next = l2;
			curr = curr->next;
			l2 = l2->next;
		}
	}

	return head;
}

vector<int> getTruthValue(ListNode* &head)
{

}


bool verifyFlattenedLinkedList(ListNode* &head, vector<int>& truthValue)
{

}

void solve(){

}
 
 
signed main(){
	ios_base::sync_with_stdio(0);
	cin.tie(0);cout.tie(0);
	int t; cin >> t; while(t--)
	solve();
	return 0;
}

// Input :
// --------------------------
// 2
// 5 6
// 4 2 2 6 4
// 5 5
// 5 6 7 8 9
//
//
// Output :
//
// 4
// 2
//