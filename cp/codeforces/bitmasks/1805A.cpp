// 1805A - We Need The Zero
// Simple implementation and bitmask

#include<bits/stdc++.h>
using namespace std;
#define int long long

void solve(){
	int n; cin >> n;

	int x = 0, a;
	for(int i=0;i<n;i++)
	{
		cin >> a;
		x ^= a;
	}

	int ans = -1;

	if(n%2 != 0 || x == 0)
	{
		ans = x;
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