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
	int ans = 0, sm=0;
	bool zero=false;

	vi arr(n,0);
	unordered_map<int,int> um;
	um[0] = -1;

	rep(i,0,n)
	{
		cin>>arr[i];
		sm += arr[i];

		if(arr[i] == 0) zero=true;

		if(um.count(sm) != 0)
			ans = max(ans, i-um[sm]);

		if(um.count(sm) == 0)
			um[sm] = i;
	}

	cout << max(ans, zero ? 1 : ans) << "\n";
}
 
 
signed main(){
	ios_base::sync_with_stdio(0);
	cin.tie(0);cout.tie(0);
	int t; cin >> t; while(t--)
	solve();
	return 0;
}


// Input : 

// 4
// 6
// 9-3 3 -1 6 -5
// 8
// 6 -2 2 -8 1 7 4 -10
// 3
// 1 0 -5
// 5
// 1 3 -5 6 -2