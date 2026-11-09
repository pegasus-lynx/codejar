#include<bits/stdc++.h>
using namespace std;

#define int         long long
#define ull         unsigned long long
#define ll             long long
#define pb             push_back
#define all(a)      (a).begin(),(a).end()
#define sz(x)       (ll)x.size()
#define endl        '\n'
#define gcd(a,b)    __gcd((a),(b))
#define lcm(a,b)    ((a)*(b)) / gcd((a),(b))
#define ios            ios_base::sync_with_stdio(false);cin.tie(0);cout.tie(0);
#define mp             make_pair
#define lb             lower_bound
#define ub            upper_bound
#define F           first
#define S           second
#define rep(i, begin, end) for (__typeof(end) i = (begin) - ((begin) > (end)); i != (end) - ((begin) > (end)); i += 1 - 2 * ((begin) > (end)))
#define hell         (ull)1e9

void solve();

signed main(void)
{
    ios
    // freopen("input.txt", "r", stdin);freopen("output.txt", "w", stdout);

    int TESTS=1;
    cin>>TESTS;

    while(TESTS--){
        solve();
    }
    return 0;
}

void solve()
{
    string s;
    cin >> s;

    vector<int> freq(26,0);
    for(char c: s)
    {
        freq[c-'a']++;
    }
    
    vector<int> tfreq;
    rep(i,0,26) if(freq[i]!=0) tfreq.pb(freq[i]);

    if(tfreq.size() < 3)
    {
        cout << "Dynamic\n";
        return;
    }
    
    sort(all(tfreq));
    int n = sz(tfreq);

    bool dyn=true;
    rep(i,2,n)
    {
        if(tfreq[i] != tfreq[i-1]+tfreq[i-2])
        {
            dyn=false;
            break;
        }
    }

    if(!dyn)
    {
        int t = tfreq[0];
        tfreq[0] = tfreq[1];
        tfreq[1] = t;
        
        dyn=true;    
        rep(i,2,n)
        {
            if(tfreq[i] != tfreq[i-1]+tfreq[i-2])
            {
                dyn=false;
                break;
            }            
        }
    }
    
    if(dyn) cout << "Dynamic\n";
    else cout << "Not\n";
}

