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
	int n, k; cin>>n>>k;
	int ans = 0, sm=0;

	vi arr(n,0);
	unordered_map<int,int> um;
	um[0] = 1;

	rep(i,0,n)
	{
		cin>>arr[i];
		sm ^= arr[i];

		if(um.count(sm^k) != 0)
			ans += um[sm^k];

		um[sm]++;
	}

	cout << ans << "\n";
}
 
 
signed main(){
	ios_base::sync_with_stdio(0);
	cin.tie(0);cout.tie(0);
	int t; cin >> t; while(t--)
	solve();
	return 0;
}