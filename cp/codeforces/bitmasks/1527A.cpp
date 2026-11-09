// 1527A - And Then There Were K
// Bitmasks

#include<bits/stdc++.h>
using namespace std;

void solve(){
	int n;
	cin >> n;

	int x=0;
	while(n)
	{
		n = n>>1;
		x = x<<1;
		x += 1; 

		// cout << n << " " << x << "\n";
	}
	x = x >> 1;
	cout << x << "\n";
}

signed main(){
	ios_base::sync_with_stdio(0);
	cin.tie(0);cout.tie(0);
	int t; cin >> t; while(t--)
	solve();
	return 0;
}