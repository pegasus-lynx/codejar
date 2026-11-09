#include<bits/stdc++.h>
using namespace std;
#define int long long
#define vi  vector<int>
#define vvi vector<vector<int>>
#define pb push_back
#define sz(x) x.size()
#define all(x) x.begin(),x.end()
#define rep(x,a,b) for(int x=a;x!=b;x = x + (b>a ? 1 : -1))
 
 
void solve(){
	int n; cin>>n;
	vector<int> arr(n,0);

	int x, flag = 0;
	for(int i=0;i<n;i++)
	{
		cin>>x;
		flag |= (x % 2) ? (x % 2) : 2;
		arr[i] = x;
	}

	if(flag == 3)
	{
		sort(arr.begin(), arr.end());
	}

	for(int i=0;i<n;i++)
	{
		cout << arr[i] << " ";
	}
	cout << "\n";
	return;
}
 
 
signed main(){
	ios_base::sync_with_stdio(0);
	cin.tie(0);cout.tie(0);
	int t; cin >> t; while(t--)
	solve();
	return 0;
}