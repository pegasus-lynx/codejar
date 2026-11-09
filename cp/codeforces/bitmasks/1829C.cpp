// 1829C - Mr. Perfectly Fine
// How bitmasks ?

#include<bits/stdc++.h>
using namespace std;
#define int long long

void solve(){
	int n; cin >> n;
	int MAXX = 10000000;
	int a[4] = {MAXX, MAXX, MAXX, MAXX};
	int h, i1, i2;
	char c1,c2;
	for(int i=0;i<n;i++)
	{
		cin >> h;
		cin >> c1 >> c2;
		int i1 = (int)c1 - (int)'0';
		int i2 = (int)c2 - (int)'0';
		int ix = i1*2 + i2;
		a[ix] = min(h, a[ix]);
	}

	// for(int i=0;i<4;i++) cout << a[i] << " ";
	// cout<<"\n";

	int ans = -1;
	if(a[3] != MAXX) ans = a[3];
	if(a[1] != MAXX && a[2] != MAXX)
	{
		ans = ans != -1 ? min(ans, a[1]+a[2]) : a[1]+a[2];
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